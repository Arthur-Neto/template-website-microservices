using System;
using Template.Domain.CommonModule;

namespace Template.Infra.Data.Crosscutting.Guard
{
    public static class Guard
    {
        public static void AgainstNull(object entity, ExceptionArguments argument)
        {
            if (entity == null)
            {
                throw new NullReferenceException(argument.ToString());
            }
        }
    }
}
