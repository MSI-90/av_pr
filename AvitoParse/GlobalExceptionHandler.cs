using AvitoParse.Models.ErrorModel;
using AvitoParse.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace AvitoParse
{
  public class GlobalExceptionHandler : IExceptionHandler
  {
    private readonly ILogger _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
      _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      httpContext.Response.ContentType = "application/json";

      var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
      if (contextFeature is not null)
      {
        switch (contextFeature.Error)
        {
          case ChangeRegionElementException:
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest; 
            break;
        }

        _logger.LogError($"Что-то пошло не так: {exception.Message}");
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
          StatusCode = httpContext.Response.StatusCode,
          Message = exception.Message,
        }.ToString());
      }

      return true;
    }
  }
}
