using StocksR.HttpClients;
using StocksR.Models;

namespace StocksR.Services;

public class StockService
{
    private readonly StocksClient stocksClient;
    
    public StockService(StocksClient client)
    {
        stocksClient = client;
    }

    public async Task<LatestStockPrice?> GetLatestStockPrice(string ticker)
    {
        try
        {
            var stockData = await stocksClient.GetStockData(ticker);
            
            if (stockData == null)
            {
             Console.WriteLine("External API was unable to retrieve latest stock data.");   
             return null;
            }

            LatestStockPrice latestPrice = new LatestStockPrice
            {
                Ticker = ticker,
                Price = decimal.Parse(stockData.Values.FirstOrDefault().Open)
            };

            return latestPrice;
        }
        catch (Exception e)
        {
            Console.WriteLine("External API was unable to retrieve latest stock price."); 
            return null;
        }
    }
}