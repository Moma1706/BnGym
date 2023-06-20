using Application;
using Infrastructure;
using Infrastructure.Data;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Urls.Add("http://38.242.149.163:5024");
app.Urls.Add("http://localhost:5024");

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
//else if (!app.Environment.IsDevelopment())
//{
//    using var scope = app.Services.CreateScope();
//    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
//    await initializer.InitializeDatabase();
//    await initializer.SeedAsync();
//}

app.UseHttpsRedirection();

//app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();