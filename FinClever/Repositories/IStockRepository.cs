using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;

namespace FinClever.Repositories
{
    public interface IStockRepository
    {
        Task<FinnhubQuote?> GetStock(string ticker);
        Task<StockHistory?> GetStockHistory(string ticker);

        Task<int> SavePriceHistoryCache(IList<HistoryStockPriceCache> prices);
        Task<HistoryStockPriceCache?> GetPriceHistoryCache(long date, string ticker);

        Task UpdateCurrentPriceCache(CurrentStockPriceCache price);
        Task<List<CurrentStockPriceCache>> GetCurrentPriceCache();
    }
}
