namespace GigBookingApi.Application.Exceptions;

public class ConflictException(string message) : GigBookingException(message, 409);