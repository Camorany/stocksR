namespace StocksR.BackgroundServices;

internal sealed class PriceUpdateOptions
{
    public TimeSpan UpdateInterval { get; set; } =  TimeSpan.FromSeconds(7.5);
}