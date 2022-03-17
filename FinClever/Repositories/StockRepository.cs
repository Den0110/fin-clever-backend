using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class StockRepository : IStockRepository
    {
        static HttpClient httpClient = new HttpClient();

        static string IexBaseUrl = "https://cloud.iexapis.com/stable";
        static string IexToken = "pk_83915c8c73ec49268860d8dd0c446299";

        private readonly FinCleverDbContext context;

        public StockRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<IexQuote?> GetStock(string ticker)
        {
            var response = await httpClient.GetAsync($"{IexBaseUrl}/stock/{ticker}/quote?token={IexToken}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IexQuote>();
            }
            return null;
        }

        public async Task<IEnumerable<IexStockHistoryItem>> GetStockHistory(string ticker)
        {
            var response = await httpClient.GetAsync($"{IexBaseUrl}/stock/{ticker}/chart/6m?token={IexToken}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<IexStockHistoryItem>>();
            }
            return new List<IexStockHistoryItem>();
        }

        public async Task SavePriceHistoryCache(IEnumerable<StockPriceCache> prices)
        {
            await context.StockPrices.AddRangeAsync(prices);
            await context.SaveChangesAsync();
        }

        public async Task<StockPriceCache> GetPriceHistoryCache(long date, string ticker)
        {
            return await context.StockPrices
                .Where(x => x.Date == date && x.Ticker == ticker)
                .FirstAsync();
        }
    }
}
