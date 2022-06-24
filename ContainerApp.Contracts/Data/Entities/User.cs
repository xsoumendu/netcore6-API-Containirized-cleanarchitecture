using ContainerApp.Contracts.Enum;

namespace ContainerApp.Contracts.Data.Entities
{
    public class User : BaseEntity
    {
        public string EmailAddress { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}