namespace Momentum.Shared.Exception;

public sealed class InternalBusinessException : System.Exception
{
    public InternalBusinessException(string message)
        : base(message)
    {

    }

    public static implicit operator InternalBusinessException(string errorMessage) =>
        new(errorMessage);
}