using Template.Domain.UsersModule;

namespace Template.Domain.TodosModule
{
    public class Todo : Entity
    {
        public string Description { get; set; }
        public int UserId { get; set; }

        // Foreign Key
        public virtual User User { get; set; }
    }
}
