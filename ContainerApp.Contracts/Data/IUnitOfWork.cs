using ContainerApp.Contracts.Data.Repositories;

namespace ContainerApp.Contracts.Data
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}