using Microsoft.EntityFrameworkCore;
using WireOps.Company.Domain;
using WireOps.Company.Domain.Common.ValueObjects;
using WireOps.Company.Infrastructure.ObjectRelationalMapping.EntityFramework;

namespace WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

public class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public DbSet<DbStaffer> Staffers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        foreach (var (type, valueType) in DomainDeepModelLayerInfo.Assembly.GetValueObjectsMeta())
            configuration.Properties(type)
                .HaveConversion(typeof(ValueObjectConverter<,>).MakeGenericType(type, valueType));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbStaffer>(staffer =>
        {
            staffer.ToTable("Staffer");

            staffer.HasKey(p => p.Id);
            staffer.Property(p => p.Email);
            staffer.Property(p => p.UserId).IsRequired(false);
            staffer.Property(p => p.GivenName);
            staffer.Property(p => p.FamilyName);
            staffer.Property(p => p.Version).IsConcurrencyToken();
        });
    }
}