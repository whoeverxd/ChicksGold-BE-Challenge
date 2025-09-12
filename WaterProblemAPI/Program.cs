var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization(); 

app.MapControllers();

// Custom 404 JSON response for undefined routes
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\": \"Route not found. Please check the API documentation. William are you trying to access a valid endpoint?, try swagger. It s free\"}");
    }
});

app.Run();

public partial class Program { }
