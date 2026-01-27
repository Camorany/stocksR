using Microsoft.AspNetCore.SignalR;
using StocksR.Models;

namespace StocksR.Hubs;

public interface IStockHubClient
{
    Task StockValueUpdated(LatestStockPrice latestStockPrice);
}

public sealed class StockValuesHub: Hub<IStockHubClient>
{
}