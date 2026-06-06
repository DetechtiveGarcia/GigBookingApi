namespace GigBookingApi.Application.Exceptions;

public abstract class GigBookingException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}