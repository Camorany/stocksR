using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using StocksR.BackgroundServices;
using StocksR.HttpClients;
using StocksR.Hubs;
using StocksR.Models;
using StocksR.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddCors();

builder.Services.AddSingleton<ActiveTickerManager>();
builder.Services.AddSingleton<PriceUpdateOptions>();
builder.Services.AddHttpClient<StocksClient>();
builder.Services.AddScoped<StockService>();
builder.Services.AddHostedService<StockPriceUpdater>();

var app = builder.Build();

app.UseCors(options => options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowCredentials()
    .AllowAnyHeader());

app.MapGet("/stockPrice/{ticker}", async (string ticker, IHubContext<StockValuesHub, 
    IStockHubClient> hubContext, StockService stockService, ActiveTickerManager tickerManager) =>
{
    LatestStockPrice? stockData = await stockService.GetLatestStockPrice(ticker);
    
    if (stockData is null)
    {
        return Results.NotFound($"No data was found for stock: {ticker}");
    }
    
    tickerManager.AddActiveTicker(ticker);
    
    await hubContext.Clients.All.StockValueUpdated(new StockPrice
        {
            Ticker  = stockData.Ticker,
            TickerPrice = stockData.Prices[stockData.Prices.Count - 1]
        }); 
    
    return Results.Ok(stockData);
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