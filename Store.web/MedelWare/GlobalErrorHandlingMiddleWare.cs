using Microsoft.AspNetCore.Http;
using Shared;

namespace Store.web.MedelWare
{
    public class GlobalErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleWare> _Logger;
        public GlobalErrorHandlingMiddleWare(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleWare> Logger)
        {
            _next = next;
            _Logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == 404)
                {
                    var response =
                    new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "The requested resource was not found."
                    };
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {

                _Logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response =
                    new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error from the custom middleware."
                    };
                response.StatusCode=ex switch
                {
                    Domain.Exceptions.NotFoundException => 404,
                    //Domain.Exceptions.BadRequestException => 400,
                    //Domain.Exceptions.UnauthorizedException => 401,
                    //_ => 500
                };
                context.Response.StatusCode = response.StatusCode;

                await context.Response.WriteAsJsonAsync(response);           }
        }



    }
}
