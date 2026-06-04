using GigBookingApi.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GigBookingApi.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate @delegate)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await @delegate(context);
        }
        catch (GigBookingException ex)
        {
            var problem = new ProblemDetails
            {
                Status = ex.StatusCode,
                Title = ex.Message
            };

            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            var problem = new ProblemDetails
            {
                Status = 500,
                Title = ex.Message
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
