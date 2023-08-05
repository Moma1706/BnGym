using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Hubs;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularOrigins",
        builder =>
        {
            builder.WithOrigins(
                                "http://localhost:4200"
                                )
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
    });

// UseCors

var app = builder.Build();
app.UseCors("AllowAngularOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeDatabase();
    await initializer.SeedAsync();
}

app.UseAuthentication();

app.UseRouting();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(60),
});

app.UseAuthorization();

// Than register your hubs here with a url.
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/hub/notification");
});

app.UseHttpsRedirection();

//app.UseExceptionHandler();
//app.MapControllers();

app.Run();