using Microsoft.AspNetCore.SignalR;
using StocksR.Models;

namespace StocksR.Hubs;

public interface IStockClient
{
    Task SendStockValueAsync(Stock stock);
}

public sealed class StockValuesHub: Hub<IStockClient>
{
}