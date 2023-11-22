using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApp.Common.Exceptions;
using WebApp.Models;
using WebApp.Common.Resources;

namespace WebApp.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        ErrorResponse error = new();

        switch (context.Exception)
        {
            case ClientException clientException:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                error = new ErrorResponse()
                {
                    HttpCode = (int)HttpStatusCode.BadRequest,
                    Message = clientException.Message,
                };
                break;
            case ServerException serverException:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error = new ErrorResponse()
                {
                    HttpCode = (int)HttpStatusCode.InternalServerError,
                    Message = Messages.GeneralErrorMessage,
                };
                break;
            default:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error = new ErrorResponse()
                {
                    HttpCode = (int)HttpStatusCode.InternalServerError,
                    Message = Messages.GeneralErrorMessage,
                };
                break;
        }

        _logger.LogError(context.Exception.StackTrace);

        ResponseContent response = new ResponseContent()
        {
            ErrorResponse = error
        };

        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }
}


