namespace TruweatherCore.Exceptions;

/// <summary>
/// Base exception for Truweather application errors.
/// </summary>
public class TruweatherException : Exception
{
    public string? ErrorCode { get; set; }

    public TruweatherException(string message) : base(message) { }

    public TruweatherException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public TruweatherException(string message, Exception innerException)
        : base(message, innerException) { }

    public TruweatherException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
