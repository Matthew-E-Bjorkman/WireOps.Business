using Microsoft.EntityFrameworkCore;
using WireOps.Company.Domain;
using WireOps.Company.Domain.Common.ValueObjects;
using WireOps.Company.Infrastructure.ObjectRelationalMapping.EntityFramework;

namespace WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

public class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
{
    public DbSet<DbStaffer> Staffers { get; set; }

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
            staffer.Property(p => p.Name);
            staffer.Property(p => p.SKU);
            staffer.Property(p => p.Description).IsRequired(false);
            staffer.Property(p => p.Version).IsConcurrencyToken();
        });
    }
}