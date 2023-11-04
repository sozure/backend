using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VGManager.Entities.Configurations;

public class EditEntityConfig : IEntityTypeConfiguration<EditEntity>
{
    /// <summary>
    /// Create configurations.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder <see cref="EntityTypeBuilder{EditEntity}"/>.</param>
    public void Configure(EntityTypeBuilder<EditEntity> builder)
    {
        throw new NotImplementedException();
    }
}
