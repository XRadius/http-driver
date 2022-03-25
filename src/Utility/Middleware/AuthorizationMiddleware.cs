using System.Text;

public class AuthorizationMiddleware : IMiddleware {
  private readonly string serverAuthorization;

  public AuthorizationMiddleware(IConfiguration configuration) {
    var username = configuration["Authorization:Username"];
    var password = configuration["Authorization:Password"];
    this.serverAuthorization = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"))}";
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    if (!context.Request.Headers.TryGetValue("Authorization", out var authorization) || authorization != serverAuthorization) {
      context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
      context.Response.Headers.Add("WWW-Authenticate", "Basic");
    } else {
      await next.Invoke(context);
    }
  }
}
