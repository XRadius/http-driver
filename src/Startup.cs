using HttpDriver.Utilities.Middleware;

namespace HttpDriver
{
    public class Startup
    {
        #region Methods

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseWebSockets();
            app.UseFileServer().UseRouting().UseEndpoints(x => x.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpForwarder();
            services.AddSingleton<AuthorizationMiddleware>();
            services.AddSingleton<ExceptionMiddleware>();
        }

        #endregion
    }
}