using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.SaleAggregate;

/// <summary>
/// SaleItem configuration
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the SaleItem entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(si => si.Id);

        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey("ProductId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(si => si.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.CreatedAt).IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.UpdatedAt).IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(si => si.IsCanceled)
            .HasDefaultValue(false);

        builder.Ignore(s => s.TotalValue);

        builder.ToTable("SaleItems");
    }
}