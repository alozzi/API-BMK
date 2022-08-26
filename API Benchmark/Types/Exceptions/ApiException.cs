namespace Benchmark.Types.Exceptions;

public class ApiException : Exception
{
    #region Constructors

    public ApiException()
    {
        
    }

    public ApiException(string message) : base(message)
    {
        
    }

    public ApiException(string message, Exception inner) : base(message, inner)
    {
        
    }

    #endregion
}