namespace GigBookingApi.Application.Exceptions;

public class NotfoundException(string message) : GigBookingException(message, 404);