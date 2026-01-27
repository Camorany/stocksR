using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
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

builder.Services.AddHttpClient<StocksClient>();
builder.Services.AddScoped<StockService>();

var app = builder.Build();

app.UseCors(options => options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowCredentials()
    .AllowAnyHeader());

app.MapGet("/", async (IHubContext<StockValuesHub, 
    IStockHubClient> hubContext, StockService stockService) =>
{
    var stockData = await stockService.GetLatestStockPrice("IBM");
    
    hubContext.Clients.All.StockValueUpdated(stockData); 
    
    return stockData;
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