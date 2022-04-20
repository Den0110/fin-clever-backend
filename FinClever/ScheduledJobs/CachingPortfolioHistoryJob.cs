using System;
using System.Linq;
using System.Threading.Tasks;
using FinClever.Models.invest;
using FinClever.Repositories;
using Quartz;

namespace FinClever
{
    [DisallowConcurrentExecution]
    public class CachingPortfolioHistoryJob : IJob
    {
        IPortfolioRepository portfolioRepository;
        IStockRepository stockRepository;

        public CachingPortfolioHistoryJob(IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Portfolio caching start");

            await UpdateHistoryFor("M", "1Y", "ALL");

            Console.WriteLine("Portfolio caching end");
        }

        private async Task UpdateHistoryFor(params string[] ranges)
        {
            await LoadNewStocks();
            foreach (var range in ranges)
            {
                Console.WriteLine($"Loading history for: {range}");
                var days = TimeUtils.GetDaysForRange(range);
                await portfolioRepository.UpdatePortfoliosCache(days, range);
            }
        }

        private async Task LoadNewStocks()
        {
            var tickers = await portfolioRepository.GetNewTickets();
            foreach (var t in tickers.Take(5))
            {
                var history = await stockRepository.GetStockHistory(t);
                if (history?.Prices != null)
                {
                    Console.WriteLine($"History size: {history.Prices.Count}");
                    var prices = history.Prices.Select(p =>
                    {
                        var date = TimeUtils.ParseDate(p.Key);
                        var price = double.Parse(p.Value.Close ?? "0");
                        return new HistoryStockPriceCache(date, t, price);
                    }).ToList();
                    var updatedPrices = await stockRepository.SavePriceHistoryCache(prices);
                    Console.WriteLine($"Updated prices: {updatedPrices}");
                }
            }
        }
    }
}

