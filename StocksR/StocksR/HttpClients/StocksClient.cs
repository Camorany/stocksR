using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using StocksR.Models;

namespace StocksR.HttpClients;

public sealed class StocksClient
{
    private readonly HttpClient _client;

    public StocksClient(HttpClient httpClient)
    {
        _client = httpClient;
        _client.BaseAddress = new Uri($"https://api.twelvedata.com/time_series?apikey={Environment.GetEnvironmentVariable("API_KEY")}" +
                                         $"&interval=1min&format=JSON&type=stock&symbol=");
    }
    
    public async Task<Stock> GetStockData(string ticker)
    {
        try
        {
            var stockData = await _client.GetStringAsync(_client.BaseAddress + ticker);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
    
            var stock = JsonSerializer.Deserialize<Stock>(stockData, options);
        
            return stock; 
        } 
        catch(Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }
    
    
}