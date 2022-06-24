using ContainerApp.Contracts.Enum;

namespace ContainerApp.Contracts.DTO
{
    public class CreateOrUpdateUserDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}