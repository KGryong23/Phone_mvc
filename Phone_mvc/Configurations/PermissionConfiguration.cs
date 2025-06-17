﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phone_mvc.Entities;
using Phone_mvc.Extensions;

namespace Phone_mvc.Configurations
{
    /// <summary>
    /// Cấu hình ánh xạ cho thực thể Permission trong Entity Framework.
    /// </summary>
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        /// <summary>
        /// Cấu hình bảng và các thuộc tính của Permission.
        /// </summary>
        /// <param name="builder">Builder để cấu hình ánh xạ.</param>
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Cấu hình bảng
            builder.ToTable("Permissions");

            // Cấu hình từ BaseDomainEntity
            builder.ConfigureBaseDomainEntity();

            // Cấu hình các thuộc tính
            builder.Property(x => x.Controller)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Endpoint)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Method)
                .IsRequired()
                .HasMaxLength(100);

            // Cấu hình quan hệ nhiều-nhiều với Role
            builder.HasMany(x => x.RolePermissions)
                .WithOne(x => x.Permission)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
