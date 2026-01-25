using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using StocksR.Hubs;
using StocksR.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(options => options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowCredentials()
    .AllowAnyHeader());

var apiKey = Environment.GetEnvironmentVariable("API_KEY");

app.MapGet("/", async (IHubContext<StockValuesHub, IStockClient> hubContext, HttpClient client) =>
{
    string queryUrl =
        $"https://api.twelvedata.com/time_series?apikey={apiKey}&interval=1min&format=JSON&type=stock&symbol=IBM";

    Uri queryUri = new Uri(queryUrl);

    var stockData = await client.GetStringAsync(queryUri);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    
    var stock = JsonSerializer.Deserialize<Stock>(stockData, options);
    
    hubContext.Clients.All.StockValueUpdated(stock); 
    
    return stock;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<StockValuesHub>("/StockValuesHub");

app.Run();