using System.Text;

namespace HttpDriver.Utilities.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly string _serverAuthorization;

        #region Constructors

        public AuthorizationMiddleware(IConfiguration configuration)
        {
            var username = configuration["Authorization:Username"];
            var password = configuration["Authorization:Password"];
            _serverAuthorization = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"))}";
        }

        #endregion

        #region Implementation of IMiddleware

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authorization) || authorization != _serverAuthorization)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.Add("WWW-Authenticate", "Basic");
            }
            else
            {
                await next.Invoke(context);
            }
        }

        #endregion
    }
}