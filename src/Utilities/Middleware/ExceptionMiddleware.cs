namespace HttpDriver.Utilities.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        #region Implementation of IMiddleware

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }

        #endregion
    }
}