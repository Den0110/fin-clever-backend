using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinClever.Models;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly FinCleverDbContext context;
        static HttpClient httpClient = new HttpClient();

        static string IexBaseUrl = "https://cloud.iexapis.com/stable";
        static string IexToken = "pk_83915c8c73ec49268860d8dd0c446299";

        public PortfolioRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PortfolioStock>> GetStocks()
        {
            var stocks = await context.InvestOperations
                .GroupBy(o => o.Ticker)
                .Select(g => MapOperationGroupToStock(g))
                .ToListAsync();

            var ticker = "AAPL";
            var response = await httpClient.GetAsync($"{IexBaseUrl}/stock/{ticker}/quote?token={IexToken}");
            if (response.IsSuccessStatusCode)
            {
                var quote = await response.Content.ReadAsAsync<IexQuote>();
                stocks.ForEach(x => x.CurrentPrice = quote.close);
            }
            response.EnsureSuccessStatusCode();

            return stocks;
        }

        private PortfolioStock MapOperationGroupToStock(IGrouping<string, InvestOperation> g)
        {
            int amount = g.Sum(x => x.Amount);
            return new PortfolioStock(
                g.Key,
                g.Sum(x => x.UsdPrice * x.Amount) / amount,
                g.Sum(x => x.Price * x.Amount) / amount,
                amount
            );
        }
    }
}
