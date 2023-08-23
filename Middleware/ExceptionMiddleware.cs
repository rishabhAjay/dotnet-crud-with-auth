using hotelListingAPI.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace hotelListingAPI.Middleware
{

    //global try catch middleware 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        //intercept the incoming request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //go to the next block of code in the context and check if you have any exceptions
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            //initialize statusCode to be that of internal server error
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            //initialize the object to have the default error type and the exception message
            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
                ErrorPath = context.Request.Path,
                ErrorTimestamp = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
            };

            //switch between cases of incoming statuses
            switch (ex)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorDetails.ErrorType = "Unauthorized";
                    break;

                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorDetails.ErrorType = "Bad Request";
                    break;
                default:
                    break;
            }
            //serialize the object to json
            string response = JsonConvert.SerializeObject(errorDetails);
            //set the status code of the response to be the changed statuscode
            context.Response.StatusCode = (int)statusCode;
            //return the response
            return context.Response.WriteAsync(response);
        }
    }
}
public class ErrorDetails
{

    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }

    public string ErrorPath { get; set; }

    public string ErrorTimestamp { get; set; }

}

