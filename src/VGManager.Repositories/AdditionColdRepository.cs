using Microsoft.EntityFrameworkCore;
using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repository;

public class AdditionColdRepository : SqlRepository<AdditionEntity>, IAdditionColdRepository
{
    public AdditionColdRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
