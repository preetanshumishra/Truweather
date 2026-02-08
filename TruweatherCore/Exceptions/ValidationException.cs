namespace TruweatherCore.Exceptions;

/// <summary>
/// Exception thrown when input validation fails.
/// </summary>
public class ValidationException : TruweatherException
{
    public Dictionary<string, string[]>? Errors { get; set; }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Dictionary<string, string[]> errors)
        : base(message)
    {
        Errors = errors;
    }
}
