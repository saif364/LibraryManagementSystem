using LibraryManagementSystem.API.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                int statusCode;

                if (ex is NotFoundException)
                    statusCode = StatusCodes.Status404NotFound;
                else if (ex is ConflictException)
                    statusCode = StatusCodes.Status409Conflict;
                else if (ex is BadRequestException)
                    statusCode = StatusCodes.Status400BadRequest;
                else
                    statusCode = StatusCodes.Status500InternalServerError;

                var isServerError = statusCode == StatusCodes.Status500InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = isServerError ? "An unexpected error occurred." : ex.Message,
                    Detail = isServerError ? null : ex.Message
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
