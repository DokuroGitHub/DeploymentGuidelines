using Application.Commons.Exceptions;
using Newtonsoft.Json;

namespace WebAPI.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Call the next middleware in the pipeline
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Exeption: {ex}");

            // Set the response status code and body based on the exception
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new
            {
                Message = "An error occurred while processing the request.",
                Details = ex.Message
            };

            // Serialize the error response
            var errorJson = JsonConvert.SerializeObject(error);

            // Write the error response to the response body
            await context.Response.WriteAsync(errorJson);
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
