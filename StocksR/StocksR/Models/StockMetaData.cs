namespace StocksR.Models;

public class StockMetaData
{
    public string Symbol { get; set; }
    public string Interval { get; set; }
    public string Currency { get; set; }
    public string ExchangeTimezone { get; set; }
    public string Exchange { get; set; }
    public string MicCode { get; set; }
    public string Type { get; set; }
}