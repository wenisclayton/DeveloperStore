using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.SaleAggregate;

/// <summary>
/// Sale entity configuration
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleNumber)
            .IsRequired();

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(e => e.CreatedAt).IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.UpdatedAt).IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(si => si.IsCanceled)
            .HasDefaultValue(false);

        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey("CustomerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Branch)
            .WithMany()
            .HasForeignKey("BranchId") 
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey("SaleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Sales");
    }
}