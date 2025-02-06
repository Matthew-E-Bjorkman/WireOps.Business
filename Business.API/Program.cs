using WireOps.Business.Application.Auth;
using Business.Application.Email;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NodaTime;
using Quartz;
using System.Security.Claims;
using WireOps.Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Application.Companies;
using WireOps.Business.Application.Companies.Create;
using WireOps.Business.Application.Companies.Get;
using WireOps.Business.Application.Companies.Update;
using WireOps.Business.Application.Roles;
using WireOps.Business.Application.Roles.Create;
using WireOps.Business.Application.Roles.Delete;
using WireOps.Business.Application.Roles.Get;
using WireOps.Business.Application.Roles.GetList;
using WireOps.Business.Application.Roles.Update;
using WireOps.Business.Application.Staffers;
using WireOps.Business.Application.Staffers.Create;
using WireOps.Business.Application.Staffers.Delete;
using WireOps.Business.Application.Staffers.DomainEvents;
using WireOps.Business.Application.Staffers.Get;
using WireOps.Business.Application.Staffers.GetList;
using WireOps.Business.Application.Staffers.Update;
using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Roles.Events;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;
using WireOps.Business.Infrastructure.Communication.Outbox.Common;
using WireOps.Business.Infrastructure.Communication.Outbox.Kafka;
using WireOps.Business.Infrastructure.Communication.Outbox.Kafka.Implementation;
using WireOps.Business.Infrastructure.Communication.Outbox.Postgres;
using WireOps.Business.Infrastructure.Communication.Outbox.Quartz;
using WireOps.Business.Infrastructure.Communication.Publisher;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;
using WireOps.Business.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigureEnvironmentVariables();
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
ConfigureEmail();

var app = builder.Build();

RegisterConfiguration();
RegisterCors();

app.UseSwagger();
app.UseSwaggerUI(x => {
    x.EnableTryItOutByDefault();
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "WireOps Business API");
    x.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await RegisterJobs();

app.Run();

void ConfigureEnvironmentVariables()
{
    builder.Configuration.AddEnvironmentVariables();
}

void ConfigureLoggers()
{
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
}

void ConfigureApiServices()
{
    builder.Services.AddControllers();

    builder.Services.AddSingleton<IClock>(SystemClock.Instance);

    builder.Services.AddHttpClient();
}

void ConfigureSwaggerDocumentation()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WireOps.Business.API", Version = "v1" });
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
    var dbHost = builder.Configuration["DB:Host"]!;
    var dbName = builder.Configuration["DB:Name"]!;
    var dbUsername = builder.Configuration["DB:Username"]!;
    var dbPassword = builder.Configuration["DB:Password"]!;
    var connectionString = $"Server={dbHost};Port=5432;Database={dbName};Username={dbUsername};Password={dbPassword};";
    builder.Services.AddDbContextPool<BusinessDbContext>(options => options
            .UseNpgsql(connectionString, npgsqlOptions => {
                npgsqlOptions.MigrationsHistoryTable("EntityFrameworkMigrationHistory");
                npgsqlOptions.UseNodaTime();
            }));

    //Company
    builder.Services.AddScoped<CompanyRepository.EntityFramework>();

    //Role
    builder.Services.AddScoped<RoleRepository.EntityFramework>();

    //Staffer
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
    builder.Services.AddSingleton<DomainEventPublisher>();

    //Company
    builder.Services.AddScoped<CompanyEventsOutbox, KafkaCompanyEventsOutbox>();

    //Role
    builder.Services.AddScoped<RoleEventsOutbox, KafkaRoleEventsOutbox>();

    //Staffer
    builder.Services.AddScoped<StafferEventsOutbox, KafkaStafferEventsOutbox>();
}

void ConfigureRepositories()
{
    //Company
    builder.Services.AddScoped<Company.Repository>(s => s.GetService<CompanyRepository.EntityFramework>()!);

    //Role
    builder.Services.AddScoped<Role.Repository>(s => s.GetService<RoleRepository.EntityFramework>()!);

    //Staffer
    builder.Services.AddScoped<Staffer.Repository>(s => s.GetService<StafferRepository.EntityFramework>()!);
}

void ConfigureFactories()
{
    //Company
    builder.Services.AddScoped<Company.Factory>(s => s.GetService<CompanyRepository.EntityFramework>()!);

    //Role
    builder.Services.AddScoped<Role.Factory>(s => s.GetService<RoleRepository.EntityFramework>()!);

    //Staffer
    builder.Services.AddScoped<Staffer.Factory>(s => s.GetService<StafferRepository.EntityFramework>()!);
}

void ConfigureHandlers()
{
    //Company
    builder.Services.AddScoped<QueryHandler<GetCompany, CompanyModel?>, GetCompanyHandler>();

    builder.Services.AddScoped<CommandHandler<CreateCompany, CompanyModel>, CreateCompanyHandler>();
    builder.Services.AddScoped<CommandHandler<UpdateCompany, CompanyModel?>, UpdateCompanyHandler>();

    //Role
    builder.Services.AddScoped<QueryHandler<GetRole, RoleModel?>, GetRoleHandler>();
    builder.Services.AddScoped<QueryHandler<GetRoleList, IReadOnlyList<RoleModel>>, GetRoleListHandler>();

    builder.Services.AddScoped<CommandHandler<CreateRole, RoleModel>, CreateRoleHandler>();
    builder.Services.AddScoped<CommandHandler<UpdateRole, RoleModel?>, UpdateRoleHandler>();
    builder.Services.AddScoped<CommandHandler<DeleteRole, bool>, DeleteRoleHandler>();

    //Staffer
    builder.Services.AddScoped<QueryHandler<GetStaffer, StafferModel?>, GetStafferHandler>();
    builder.Services.AddScoped<QueryHandler<GetStafferList, IReadOnlyList<StafferModel>>, GetStafferListHandler>();

    builder.Services.AddScoped<CommandHandler<CreateStaffer, StafferModel>, CreateStafferHandler>();
    builder.Services.AddScoped<CommandHandler<UpdateStaffer, StafferModel?>, UpdateStafferHandler>();
    builder.Services.AddScoped<CommandHandler<DeleteStaffer, bool>, DeleteStafferHandler>();
    builder.Services.AddScoped<CommandHandler<LinkUserToStaffer, StafferModel?>, LinkUserToStafferHandler>();
    builder.Services.AddScoped<CommandHandler<InviteStaffer, StafferModel?>, InviteStafferHandler>();

    builder.Services.AddScoped<DomainEventHandler<RolePermissionsChanged>, RolePermissionsChangedHandler>();
    builder.Services.AddScoped<DomainEventHandler<StafferRoleAssigned>, StafferRoleAssignedHandler>();
}

void ConfigureDecorators()
{
    builder.Services.TryDecorate(typeof(CommandHandler<>), typeof(AmbientTransactionDecorator<>));
    builder.Services.TryDecorate(typeof(CommandHandler<,>), typeof(AmbientTransactionDecorator<,>));
}

void ConfigureJobScheduling()
{
    return; //TODO: Implement, probably as a separate service for AWS (so api services aren't running nonstop)

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
            "read:companies",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("read:companies", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "write:companies",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("write:companies", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "read:roles",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("read:roles", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "write:roles",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("write:roles", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "read:staffers",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("read:staffers", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "write:staffers",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("write:staffers", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
        options.AddPolicy(
            "admin",
            policy => {
                policy.Requirements.Add(new HasScopeRequirement("admin", $"https://{domain}/"));
                policy.Requirements.Add(new BelongsToCompanyRequirement());
            }
        );
    });

    builder.Services.AddSingleton<IActionContextAccessor,  ActionContextAccessor>();
    builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, BelongsToCompanyHandler>();

    builder.Services.AddScoped<Auth0APIClient>();
}

void ConfigureEmail()
{
    builder.Services.AddSingleton<IEmailClient, MailgunEmailClient>();
}

void RegisterConfiguration()
{
    app.Services.GetRequiredService<MessageTypes>().Register<CompanyEvent>("CompanyId", new List<string>()); //TODO: This is probably very wrong  
    app.Services.GetRequiredService<MessageTypes>().Register<RoleEvent>("RoleId", new List<string>()); //TODO: This is probably very wrong  
    app.Services.GetRequiredService<MessageTypes>().Register<StafferEvent>("StafferId", new List<string>()); //TODO: This is probably very wrong  
}

async Task RegisterJobs()
{
    return; //TODO: Implement, probably as a separate service for AWS (so api services aren't running nonstop)

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

void RegisterCors()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseCors(builder => {
            builder.WithOrigins("https://local.ui:3000") // WireOps.UI
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    }

    if (app.Environment.IsProduction())
    {
        app.UseCors(builder => {
            builder.WithOrigins("https://www.invensync.com") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();

            builder.WithOrigins("https://invensync.com")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    }
}