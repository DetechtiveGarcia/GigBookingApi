namespace GigBookingApi.Application.Exceptions;

public class NotFoundException(string message) : GigBookingException(message, 404);