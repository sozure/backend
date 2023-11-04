using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VGManager.Entities.Configurations;

public class DeletionEntityConfig : IEntityTypeConfiguration<DeletionEntity>
{
    /// <summary>
    /// Create configurations.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder <see cref="EntityTypeBuilder{DeletionEntity}"/>.</param>
    public void Configure(EntityTypeBuilder<DeletionEntity> builder)
    {
        throw new NotImplementedException();
    }
}
