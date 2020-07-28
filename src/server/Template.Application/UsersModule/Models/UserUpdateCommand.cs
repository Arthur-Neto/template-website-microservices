namespace Template.Application.UsersModule.Models
{
    public class UserUpdateCommand
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
