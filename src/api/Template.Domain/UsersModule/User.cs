using Template.Domain.UsersModule.Enums;

namespace Template.Domain.UsersModule
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
