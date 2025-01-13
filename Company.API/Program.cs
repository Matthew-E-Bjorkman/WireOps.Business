using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Security.Claims;
using WireOps.Company.Application.Auth;
using WireOps.Company.Application.Common;
using WireOps.Company.Application.Staffers;
using WireOps.Company.Application.Staffers.Create;
using WireOps.Company.Application.Staffers.Delete;
using WireOps.Company.Application.Staffers.Get;
using WireOps.Company.Application.Staffers.GetList;
using WireOps.Company.Application.Staffers.Update;
using WireOps.Company.Domain.Common.Definitions;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Domain.Staffers.Events;
using WireOps.Company.Infrastructure.Communication.Outbox.Common;
using WireOps.Company.Infrastructure.Communication.Outbox.Kafka;
using WireOps.Company.Infrastructure.Communication.Outbox.Kafka.Implementation;
using WireOps.Company.Infrastructure.Communication.Outbox.Postgres;
using WireOps.Company.Infrastructure.Communication.Outbox.Quartz;
using WireOps.Company.Infrastructure.Database.SQL.EntityFramework;
using WireOps.Company.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigureLoggers();
ConfigureApiServices();
ConfigureSwaggerDocumentation();
ConfigurePersistence();
ConfigureCommunication();
ConfigureRepositories();
ConfigureFactories();
ConfigureHandlers();
ConfigureDecorators();
ConfigureJobScheduling();
ConfigureAuth0();

var app = builder.Build();

RegisterConfiguration();
RegisterCorsForLocalDevelopment();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => {
        x.EnableTryItOutByDefault();
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await RegisterJobs();

app.Run();

void ConfigureLoggers()
{
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
}

void ConfigureApiServices()
{
    builder.Services.AddControllers();
}

void ConfigureSwaggerDocumentation()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WireOps.Company.API", Version = "v1" });
        var securitySchema = new OpenApiSecurityScheme
        {
            Description = "JWT Auth Bearer Scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        config.AddSecurityDefinition("Bearer", securitySchema);
        var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
        config.AddSecurityRequirement(securityRequirement);
    });
}

void ConfigurePersistence()
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContextPool<CompanyDbContext>(options => options
            .UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable("EntityFrameworkMigrationHistory")));

    builder.Services.AddScoped<StafferRepository.EntityFramework>();
}

void ConfigureCommunication()
{
    builder.Services.AddSingleton<MessageTypes>();
    builder.Services.AddScoped<TransactionalOutboxes>();
    builder.Services.AddScoped<TransactionalOutboxRepository, PostgresOutboxRepository>();
    builder.Services.AddScoped<PostgresOutboxRepository>();
    builder.Services.AddScoped<PostgresOutboxProcessorSettings>();
    builder.Services.AddScoped<ProducerConfig>();
    builder.Services.AddScoped<KafkaMessageProducer>();
    builder.Services.AddScoped<OutboxMessageProcessor, KafkaOutboxMessageProcessor>();
    builder.Services.AddScoped<StafferEventsOutbox, KafkaStafferEventsOutbox>();
}

void ConfigureRepositories()
{
    builder.Services.AddScoped<Staffer.Repository>(s => s.GetService<StafferRepository.EntityFramework>()!);
}

void ConfigureFactories()
{
    builder.Services.AddScoped<Staffer.Factory>(s => s.GetService<StafferRepository.EntityFramework>()!);
}

void ConfigureHandlers()
{
    builder.Services.AddScoped<QueryHandler<GetStaffer, StafferModel?>, GetStafferHandler>();
    builder.Services.AddScoped<QueryHandler<GetStafferList, IReadOnlyList<StafferModel>>, GetStafferListHandler>();

    builder.Services.AddScoped<CommandHandler<CreateStaffer, StafferModel>, CreateStafferHandler>();
    builder.Services.AddScoped<CommandHandler<UpdateStaffer, StafferModel?>, UpdateStafferHandler>();
    builder.Services.AddScoped<CommandHandler<DeleteStaffer, bool>, DeleteStafferHandler>();
}

void ConfigureDecorators()
{
    builder.Services.TryDecorate(typeof(CommandHandler<>), typeof(AmbientTransactionDecorator<>));
    builder.Services.TryDecorate(typeof(CommandHandler<,>), typeof(AmbientTransactionDecorator<,>));
}

void ConfigureJobScheduling()
{
    builder.Services.AddScoped<PostgresOutboxProcessor>();

    builder.Services.AddQuartz();
    builder.Services.AddQuartzHostedService(opt =>
    {
        opt.WaitForJobsToComplete = true;
    });
}

void ConfigureAuth0()
{
    var domain = builder.Configuration["Auth0:Domain"]!;

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{domain}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(
            "read:staffers",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("read:staffers", $"https://{domain}/")
            )
        );
        options.AddPolicy(
            "write:staffers",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("write:staffers", $"https://{domain}/")
            )
        );
        options.AddPolicy(
            "admin",
            policy => policy.Requirements.Add(
                new HasScopeRequirement("admin", $"https://{domain}/")
            )
        );
    });

    builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
}

void RegisterConfiguration()
{
    app.Services.GetRequiredService<MessageTypes>().Register<StafferEvent>("StafferId", new List<string>()); //TODO: This is probably very wrong  
}

async Task RegisterJobs()
{
    var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
    var scheduler = await schedulerFactory.GetScheduler();

    // define the job and tie it to our HelloJob class
    var job = JobBuilder.Create<OutboxJob<PostgresOutboxProcessor>>()
        .WithIdentity("OutboxJob")
        .Build();

    // Trigger the job to run now, and then every 40 seconds
    var trigger = TriggerBuilder.Create()
        .WithIdentity("OutboxJobTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(5)
            .RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);
}

void RegisterCorsForLocalDevelopment()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseCors(builder =>
            builder.WithOrigins("http://localhost:3000") // WireOps.Company.UI
                   .AllowAnyHeader()
                   .AllowAnyMethod()
        );
    }
}