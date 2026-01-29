using Microsoft.AspNetCore.SignalR;
using StocksR.Models;

namespace StocksR.Hubs;

public interface IStockHubClient
{
    Task StockValueUpdated(StockPrice stockPrice);
}

public sealed class StockValuesHub: Hub<IStockHubClient>
{
}