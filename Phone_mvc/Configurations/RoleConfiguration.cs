using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phone_mvc.Entities;
using Phone_mvc.Extensions;

namespace Phone_mvc.Configurations
{
    /// <summary>
    /// Cấu hình ánh xạ cho thực thể Role trong Entity Framework.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Cấu hình bảng và các thuộc tính của Role.
        /// </summary>
        /// <param name="builder">Builder để cấu hình ánh xạ.</param>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Cấu hình bảng
            builder.ToTable("Roles");

            // Cấu hình từ BaseDomainEntity
            builder.ConfigureBaseDomainEntity();

            // Cấu hình các thuộc tính
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Cấu hình quan hệ nhiều-nhiều với User qua UserRole
            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ nhiều-nhiều với Permission
            builder.HasMany(x => x.RolePermissions)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
