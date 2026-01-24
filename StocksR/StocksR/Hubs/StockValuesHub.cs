using Microsoft.AspNetCore.SignalR;
using StocksR.Models;

namespace StocksR.Hubs;

public sealed class StockValuesHub: Hub
{
    public async Task SendStockValueAsync(Stock stock)
    {
        await Clients.Caller.SendAsync("ReceiveStockValue", stock.Value);
    } 
    
}