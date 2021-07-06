namespace Template.Infra.Crosscutting.Exceptions
{
    public enum ErrorType
    {
        NotFound,
        Duplicating,
        IncorrectUserPassword,
        IDShouldBeGreaterThanZero,
        FailToAutenticateUser,
        SecretKeyTooShort,
        SaveChangesFailure,
        EnterpriseNotFound
    }
}
