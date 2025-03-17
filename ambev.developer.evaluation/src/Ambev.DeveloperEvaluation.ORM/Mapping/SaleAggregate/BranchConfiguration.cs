using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.SaleAggregate;


/// <summary>
/// Represents the configuration of the Branch entity
/// </summary>
public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{

    /// <summary>
    /// Configures the entity of type Branch
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Branch> builder)
    {

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.ToTable("Branches");
    }
}