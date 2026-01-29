namespace StocksR.Models;

public class LatestStockPrice
{
    public string Ticker { get; set; }
    public IReadOnlyList<decimal> Prices { get; set; }
    
}