using System.Collections.Generic;
using Template.Domain.TodosModule;

namespace Template.Domain.UsersModule
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }

        // Reverse Navigation
        public virtual IEnumerable<Todo> Todos { get; set; }
    }
}
