using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using StocksR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();
var apiKey = Environment.GetEnvironmentVariable("API_KEY");

app.MapGet("/", (IHubContext<StockValuesHub, IStockClient> hubContext) =>
{
    string QUERY_URL =
        $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=AAPL&apikey={apiKey}";
    
        Uri queryUri = new Uri(QUERY_URL);
    
        using (WebClient client = new WebClient())
        { 
            dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
            return json_data;
        }
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
