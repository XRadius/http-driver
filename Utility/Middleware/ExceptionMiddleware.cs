public class ExceptionMiddleware : IMiddleware {
  private ILogger<ExceptionMiddleware> loggerService;

  public ExceptionMiddleware(ILogger<ExceptionMiddleware> loggerService) {
    this.loggerService = loggerService;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    try {
      await next.Invoke(context);
    } catch (Exception ex) {
      context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
      loggerService.LogError(ex, ex.Message);
    }
  }
}
