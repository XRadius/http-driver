// Initialize the services.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<AuthorizationMiddleware>();
builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddSingleton<MemoryService>();
builder.Services.AddOpenApiDocument(x => x.Title = "http-driver");

// Initialize the application.
var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// Initialize the application routes.
app.UseOpenApi();
app.UseRouting();
app.UseSwaggerUi3();
app.UseEndpoints(x => x.MapControllers());
app.Run();
