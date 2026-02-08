namespace TruweatherCore.Exceptions;

/// <summary>
/// Exception thrown when API communication fails or returns an error.
/// </summary>
public class ApiException : TruweatherException
{
    public int? StatusCode { get; set; }
    public string? ResponseContent { get; set; }

    public ApiException(string message) : base(message) { }

    public ApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(string message, int statusCode, string responseContent)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }

    public ApiException(string message, Exception innerException)
        : base(message, innerException) { }
}
