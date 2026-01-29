using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StocksR.Hubs;
using StocksR.Models;
using StocksR.Services;

namespace StocksR.BackgroundServices;

internal sealed class StockPriceUpdater(
    IHubContext<StockValuesHub, IStockHubClient> hubContext,
    IServiceScopeFactory serviceScopeFactory,
    PriceUpdateOptions options,
    ActiveTickerManager tickerManager,
    IMemoryCache cache): BackgroundService
{
    private int _index = -1;
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
        if (tickerManager.GetAllActiveTickers().Count > 0)
        {
            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                StockService stockService = serviceScope.ServiceProvider.GetRequiredService<StockService>();

                foreach (var ticker in tickerManager.GetAllActiveTickers())
                {
                    
                    if (cache.TryGetValue<List<decimal>>(ticker, out var latestPrice))
                    {
                        if (_index == -1)
                        {
                            _index = latestPrice.Count - 2;
                        }
                        await hubContext.Clients.All.StockValueUpdated(
                            new StockPrice
                            {
                                Ticker  = ticker,
                                TickerPrice = latestPrice[_index]
                            });
                    }
                    else
                    {
                        LatestStockPrice? tickerStockPrices = await stockService.GetLatestStockPrice(ticker);
                        if (tickerStockPrices == null)
                        {
                            continue;
                        }
                        
                        if (_index == -1)
                        {
                            _index = tickerStockPrices.Prices.Count - 2;
                        }
                
                        await hubContext.Clients.All.StockValueUpdated(
                            new StockPrice
                            {
                                Ticker  = tickerStockPrices.Ticker,
                                TickerPrice = tickerStockPrices.Prices[_index]
                            });
                    }
                    
                }

                _index -= 1;
            }
        }
    }
}