using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Infra.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Name)
                .HasMaxLength(80)
                .IsRequired()
                .HasColumnType("varchar(80)")
                .HasColumnName("Name");

            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.Img)
                .IsRequired()
                .HasColumnType("varchar(100)");
        }
    }
}
