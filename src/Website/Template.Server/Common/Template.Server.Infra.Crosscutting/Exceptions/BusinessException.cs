using System.Runtime.Serialization;

namespace Template.Server.Infra.Crosscutting.Exceptions;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException()
    { }

    public BusinessException(string error)
        : base(error)
    { }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    { }

    protected BusinessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }

    public override string StackTrace => base.StackTrace ?? string.Empty;
}
