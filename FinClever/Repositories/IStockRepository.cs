using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;

namespace FinClever.Repositories
{
    public interface IStockRepository
    {
        Task<IexQuote> GetStock(string ticker);
        Task<IEnumerable<IexStockHistoryItem>> GetStockHistory(string ticker);
        Task SavePriceHistoryCache(IEnumerable<StockPriceCache> prices);
        Task<StockPriceCache> GetPriceHistoryCache(long date, string ticker);
    }
}
