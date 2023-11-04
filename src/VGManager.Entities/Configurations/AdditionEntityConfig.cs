using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VGManager.Entities.Configurations;

public class AdditionEntityConfig : IEntityTypeConfiguration<AdditionEntity>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder">EntityTypeBuilder <see cref="EntityTypeBuilder{AdditionEntity}"/>.</param>
    public void Configure(EntityTypeBuilder<AdditionEntity> builder)
    {
        throw new NotImplementedException();
    }
}
