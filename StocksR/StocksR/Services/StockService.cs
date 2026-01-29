using Microsoft.Extensions.Caching.Memory;
using StocksR.HttpClients;
using StocksR.Models;

namespace StocksR.Services;

public class StockService
{
    private readonly StocksClient _stocksClient;
    private readonly IMemoryCache _cache;
    
    public StockService(StocksClient client, IMemoryCache cache)
    {
        _stocksClient = client;
        _cache = cache;
    }

    public async Task<LatestStockPrice?> GetLatestStockPrice(string ticker)
    {
        try
        {
            var stockData = await _stocksClient.GetStockData(ticker);
            
            if (stockData == null)
            {
             Console.WriteLine("External API was unable to retrieve latest stock data.");   
             return null;
            }

            LatestStockPrice latestPrice = new LatestStockPrice
            {
                Ticker = ticker,
                Prices = stockData.Values.Select(v => decimal.Parse(v.Open)).ToList()
            };
            
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
            _cache.Set(ticker, latestPrice.Prices, cacheEntryOptions);

            return latestPrice;
        }
        catch (Exception e)
        {
            Console.WriteLine("External API was unable to retrieve latest stock price."); 
            return null;
        }
    }
}