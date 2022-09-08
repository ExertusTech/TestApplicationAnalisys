namespace Common.Web;

public enum Severity
{
    Warning = 1,
    Error = 2,
    Critical = 3
}

public class Envelope<T>
{
    public T? Result { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime TimeGenerated { get; set; }
    public Severity Severity { get; set; }

    protected internal Envelope(T? result, string errorMessage)
    {
        Result = result;
        ErrorMessage = errorMessage;
        TimeGenerated = DateTime.UtcNow;
    }

    protected internal Envelope(T? result, string errorMessage, Severity severity)
        : this(result, errorMessage)
    {
        this.Severity = severity;
    }
}

public class Envelope : Envelope<string>
{
    protected Envelope(string errorMessage, Severity severity)
        : base(null, errorMessage, severity)
    {
    }

    protected Envelope(string errorMessage)
        : base(null, errorMessage)
    {
    }

    public static Envelope<T> Ok<T>(T result)
    {
        return new Envelope<T>(result, "");
    }

    public static Envelope Ok()
    {
        return new Envelope("");
    }

    public static Envelope Error(string errorMessage, Severity severity)
    {
        return new Envelope(errorMessage, severity);
    }
}