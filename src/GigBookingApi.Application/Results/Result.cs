namespace GigBookingApi.Application.Results;
public sealed record Result<T>(bool Succeeded, T? Value = default, string? ErrorMessage = null)
{
    public static Result<T> Success(T value) => new(true, Value: value);
    public static Result<T> Fail(string errorMessage) => new(false, ErrorMessage: errorMessage);
}

public sealed record Result(bool Succeeded, string? ErrorMessage = null)
{
    public static Result Success() => new(true);
    public static Result Fail(string errorMessage) => new(false, ErrorMessage: errorMessage);
}