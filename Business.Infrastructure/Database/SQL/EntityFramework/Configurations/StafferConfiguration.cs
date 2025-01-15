using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Configurations;

public class StafferConfiguration : IEntityTypeConfiguration<DbStaffer>
{
    public void Configure(EntityTypeBuilder<DbStaffer> stafferConfiguration)
    {
        stafferConfiguration.ToTable("Staffer");

        stafferConfiguration.HasKey(s => s.Id);
        stafferConfiguration.Property(s => s.Email);
        stafferConfiguration.Property(s => s.UserId).IsRequired(false);
        stafferConfiguration.Property(s => s.GivenName);
        stafferConfiguration.Property(s => s.FamilyName);
        stafferConfiguration.Property(s => s.IsOwner);
        stafferConfiguration.Property(s => s.Version).IsConcurrencyToken();

        stafferConfiguration.HasOne<DbCompany>()
            .WithMany()
            .HasForeignKey(s => s.CompanyId)
            .IsRequired();
    }
}
