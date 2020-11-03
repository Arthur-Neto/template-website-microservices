using Template.Infra.Crosscutting.Exceptions;

namespace Template.Infra.Crosscutting.Guards
{
    public static class Guard
    {
        public static void Against(object entity, ErrorType type)
        {
            if (entity == null)
            {
                throw new GuardException(type.ToString());
            }
        }

        public static void Against(bool assertion, ErrorType type)
        {
            if (assertion)
            {
                throw new GuardException(type.ToString());
            }
        }
    }
}
