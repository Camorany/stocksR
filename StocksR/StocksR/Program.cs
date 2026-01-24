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
builder.Services.AddHttpClient();

var app = builder.Build();
var apiKey = Environment.GetEnvironmentVariable("API_KEY");

app.MapGet("/", async (IHubContext<StockValuesHub, IStockClient> hubContext, HttpClient client) =>
{
    string queryUrl =
        $"https://api.twelvedata.com/time_series?apikey={apiKey}&interval=1min&format=JSON&type=stock&symbol=MSFT";

    Uri queryUri = new Uri(queryUrl);

    var jsonData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(await client.GetStringAsync(queryUri));
    return jsonData;
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