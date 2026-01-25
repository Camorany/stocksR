namespace StocksR.Models;

public class StockMetaData
{
    public string Symbol { get; set; }
    public string Interval { get; set; }
    public string Currency { get; set; }
    public string Exchange_Timezone { get; set; }
    public string Exchange { get; set; }
    public string Mic_Code { get; set; }
    public string Type { get; set; }
}