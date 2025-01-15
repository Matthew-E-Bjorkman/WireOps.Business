using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<DbCompany>
{
    public void Configure(EntityTypeBuilder<DbCompany> companyConfiguration)
    {
        companyConfiguration.ToTable("Company");

        companyConfiguration.HasKey(c => c.Id);
        companyConfiguration.Property(c => c.Name);
        companyConfiguration.Property(c => c.Version).IsConcurrencyToken();

        //Address value object persisted as owned entity
        companyConfiguration.OwnsOne(c => c.Address)
            .WithOwner()
            .HasForeignKey("CompanyId");
            
        companyConfiguration.Navigation(c => c.Address).IsRequired(false);
    }
}
