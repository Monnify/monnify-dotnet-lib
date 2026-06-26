namespace Monnify.Exceptions;

/// <summary>Base type for all exceptions thrown by the Monnify SDK.</summary>
public abstract class MonnifyException : Exception
{
    protected MonnifyException(string message) : base(message) { }

    protected MonnifyException(string message, Exception innerException) : base(message, innerException) { }
}
