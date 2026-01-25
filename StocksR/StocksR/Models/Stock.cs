
namespace StocksR.Models;

public class Stock
{
    public StockMetaData Meta { get; set; }
    public List<StockData> Values { get; set; }
    public string Status { get; set; }
}