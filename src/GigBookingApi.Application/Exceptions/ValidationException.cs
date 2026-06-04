namespace GigBookingApi.Application.Exceptions;

public class ValidationException(string message) : GigBookingException(message, 400);