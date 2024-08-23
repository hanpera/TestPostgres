using TestPostgres.ApiService;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestPostgres.ApiService.Options;
using TestPostgres.ApiService.Services;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<ItemContext>(options =>
//        options.UseNpgsql(builder.Configuration.GetConnectionString("TestDB"), o => o.UseVector()));
builder.AddNpgsqlDbContext<ItemContext>("TestDB", null,
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("TestDB"), o => o.UseVector()));
// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.Configure<SemanticKernel>(builder.Configuration.GetSection("SemanticKernel"));
builder.Services.Configure<Chat>(builder.Configuration.GetSection("Chat"));
builder.Services.AddScoped<IDBService, PostgresDBService>();
builder.Services.AddScoped<ISemanticKernelService, SemanticKernelService>();
builder.Services.AddScoped< ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
}).WithName("GetWeatherForecast").WithOpenApi();

app.MapGet("/testpostgres", (ItemContext context) =>
{
    context.Database.EnsureCreated();
    var items = context.Cache.ToList();
    return items;
}).WithName("GetTestPostgres").WithOpenApi();


app.MapGet("/checkIntegrity", async (ChatService service) =>
{
    //context.Database.EnsureCreated();
    //var items = context.Cache.ToList();
    var session = await service.CreateNewChatSessionAsync();
    var message = await service.GetChatCompletionAsync(session.Id, "Hello, how are you?");


    return Results.Ok();
}).WithName("GetCheckIntegrity").WithOpenApi();

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
