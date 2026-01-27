using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using StocksR.Hubs;
using StocksR.Models;
using StocksR.Services;

namespace StocksR.BackgroundServices;

internal sealed class StockPriceUpdater(
    IHubContext<StockValuesHub, IStockHubClient> hubContext,
    IServiceScopeFactory serviceScopeFactory,
    PriceUpdateOptions options,
    ActiveTickerManager tickerManager): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UpdateStockPrices();
            await Task.Delay(options.UpdateInterval, token);
        }
    }

    private async Task UpdateStockPrices()
    {
        using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
        {
            StockService stockService = serviceScope.ServiceProvider.GetRequiredService<StockService>();

            foreach (var ticker in tickerManager.GetAllActiveTickers())
            {
                LatestStockPrice? tickerStockPrice = await stockService.GetLatestStockPrice(ticker);
                if (tickerStockPrice == null)
                {
                    continue;
                }
                
                await hubContext.Clients.All.StockValueUpdated(tickerStockPrice);
            }
            
        }
    }
}