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

        static string FinnhubBaseUrl = "https://finnhub.io/api/v1";
        static string FinnhubToken = "c8ps1kaad3icps1jv6eg";

        static string AvBaseUrl = "https://www.alphavantage.co/query";
        static string AvApiKey = "EQWYQ76Z4AVWLZCZ";

        private readonly FinCleverDbContext context;

        public StockRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<FinnhubQuote?> GetStock(string ticker)
        {
            var response = await httpClient.GetAsync($"{FinnhubBaseUrl}/quote?symbol={ticker}&token={FinnhubToken}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<FinnhubQuote>();
            }
            return null;
        }

        public async Task<StockHistory?> GetStockHistory(string ticker)
        {
            var response = await httpClient.GetAsync($"{AvBaseUrl}?function=TIME_SERIES_DAILY&outputsize=full&symbol={ticker}&apikey={AvApiKey}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<StockHistory>();
            }
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return null;
        }

        public async Task<int> SavePriceHistoryCache(IList<StockPriceCache> prices)
        {
            foreach(var t in prices.Select(x => x.Ticker).Distinct())
            {
                context.StockPrices.RemoveRange(context.StockPrices.Where(x => x.Ticker == t));
            }

            context.StockPrices.AddRange(prices);

            return await context.SaveChangesAsync();
        }

        public async Task<StockPriceCache?> GetPriceHistoryCache(long date, string ticker)
        {
            return await context.StockPrices
                .Where(x => x.Date == date && x.Ticker == ticker)
                .FirstOrDefaultAsync();
        }
    }
}
