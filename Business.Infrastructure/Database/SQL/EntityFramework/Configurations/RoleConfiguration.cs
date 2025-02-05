using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireOps.Business.Domain.Roles;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<DbRole>
{
    public void Configure(EntityTypeBuilder<DbRole> roleConfiguration)
    {
        roleConfiguration.ToTable("Role");

        roleConfiguration.HasKey(r => r.Id);
        roleConfiguration.Property(r => r.Name);
        roleConfiguration.Property(r => r.IsAdmin);
        roleConfiguration.Property(r => r.IsOwnerRole);
        roleConfiguration.Property(r => r.Version).IsConcurrencyToken();

        roleConfiguration.OwnsMany(r => r.Permissions, permissionConfiguration =>
        {
            permissionConfiguration.ToTable("RolePermission");
            permissionConfiguration.WithOwner().HasForeignKey("RoleId");
            permissionConfiguration.Property(p => p.Resource);
            permissionConfiguration.Property(p => p.Action).HasConversion<EnumToStringConverter<ResourceAction>>();
        });

        roleConfiguration.HasOne<DbCompany>()
            .WithMany()
            .HasForeignKey(r => r.CompanyId)
            .IsRequired();
    }
}
