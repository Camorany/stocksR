using System.Collections.Concurrent;

namespace StocksR.BackgroundServices;

internal sealed class ActiveTickerManager
{
    private readonly ConcurrentBag<string> _activeTickers = [];

    public void AddActiveTicker(string ticker)
    {
        if (!_activeTickers.Contains(ticker))
        {
            _activeTickers.Add(ticker);
        }
    }

    public IReadOnlyCollection<string> GetAllActiveTickers()
    {
        return _activeTickers.ToList();
    }
}