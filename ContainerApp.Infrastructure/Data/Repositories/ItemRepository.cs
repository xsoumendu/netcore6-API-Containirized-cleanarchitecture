using ContainerApp.Contracts.Data.Entities;
using ContainerApp.Contracts.Data.Repositories;
using ContainerApp.Infrastructure.Data.Repositories.Generic;
using ContainerApp.Migrations;

namespace ContainerApp.Infrastructure.Data.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DatabaseContext context) : base(context)
        {
        }
    }
}