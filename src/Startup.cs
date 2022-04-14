public class Startup {
  public void ConfigureServices(IServiceCollection services) {
    services.AddControllers();
    services.AddHttpForwarder();
    services.AddSingleton<AuthorizationMiddleware>();
    services.AddSingleton<ExceptionMiddleware>();
    services.AddSingleton<MemoryService>();
  }

  public void Configure(IApplicationBuilder app) {
    app.UseMiddleware<AuthorizationMiddleware>();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseFileServer().UseRouting().UseEndpoints(x => x.MapControllers());
  }
}
