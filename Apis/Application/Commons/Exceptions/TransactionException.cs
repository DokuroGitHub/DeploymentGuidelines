namespace Application.Commons.Exceptions;

public class TransactionException : Exception
{
    public TransactionException() : base()
    {
    }
    public TransactionException(string message)
    : base(message)
    {
    }

    public TransactionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public TransactionException(string name, object key)
        : base($"Reposity {name} ({key}) threw exception.")
    {
    }
}
