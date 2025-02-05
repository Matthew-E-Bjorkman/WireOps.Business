using Business.Infrastructure.Database.SQL.EntityFramework.Common;
using Business.Infrastructure.Database.SQL.EntityFramework.Configurations;
using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Threading;
using WireOps.Business.Domain;
using WireOps.Business.Domain.Common.ValueObjects;
using WireOps.Business.Infrastructure.ObjectRelationalMapping.EntityFramework;

namespace WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

public class BusinessDbContext(DbContextOptions<BusinessDbContext> options, IClock clock) : DbContext(options)
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public DbSet<DbStaffer> Staffers { get; set; }
    public DbSet<DbCompany> Companies { get; set; }
    public DbSet<DbRole> Roles { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        foreach (var (type, valueType) in DomainDeepModelLayerInfo.Assembly.GetValueObjectsMeta())
            configuration.Properties(type)
                .HaveConversion(typeof(ValueObjectConverter<,>).MakeGenericType(type, valueType));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StafferConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        AddTimestamps();
        return this.SaveChangesAsync(CancellationToken.None);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is DbObject && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = clock.GetCurrentInstant(); 

            if (entity.State == EntityState.Added)
            {
                ((DbObject)entity.Entity).CreatedAt = now;
            }
            ((DbObject)entity.Entity).ModifiedAt = now;
        }
    }
}