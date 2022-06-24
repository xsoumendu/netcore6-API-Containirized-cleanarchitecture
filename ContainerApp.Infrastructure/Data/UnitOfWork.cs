using ContainerApp.Contracts.Data;
using ContainerApp.Contracts.Data.Repositories;
using ContainerApp.Infrastructure.Data.Repositories;
using ContainerApp.Migrations;

namespace ContainerApp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        public IItemRepository Items => new ItemRepository(_context);

        public IUserRepository Users => new UserRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}