using ContainerApp.Contracts.Data.Entities;
using ContainerApp.Contracts.DTO;

namespace ContainerApp.Contracts.Services
{
    public interface ITokenService
    {
        AuthTokenDTO Generate(User user);
    }
}