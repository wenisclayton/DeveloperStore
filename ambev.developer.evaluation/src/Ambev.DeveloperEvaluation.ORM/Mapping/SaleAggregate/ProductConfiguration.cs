using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.SaleAggregate;


/// <summary>
/// Represents the mapping configuration for the Product entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the entity mapping.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.ToTable("Products");
    }
}