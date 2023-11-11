using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VGManager.Entities.Configurations;

public class EditionEntityConfig : IEntityTypeConfiguration<EditionEntity>
{
    /// <summary>
    /// Create configurations.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder <see cref="EntityTypeBuilder{EditionEntity}"/>.</param>
    public void Configure(EntityTypeBuilder<EditionEntity> builder)
    {
        builder.HasKey(editon => editon.Id);
        builder.Property(editon => editon.Id).ValueGeneratedOnAdd();
        builder.ToTable("Editions");
    }
}
