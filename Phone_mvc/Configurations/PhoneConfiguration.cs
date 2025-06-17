using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phone_mvc.Entities;
using Phone_mvc.Extensions;

namespace Phone_mvc.Configurations
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            // Cấu hình bảng
            builder.ToTable("Phones");

            // Cấu hình từ BaseDomainEntity
            builder.ConfigureBaseDomainEntity();

            // Cấu hình các thuộc tính
            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Stock)
                .IsRequired();

            // Cấu hình khóa ngoại
            builder.HasOne(x => x.Brand)
                .WithMany()
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
