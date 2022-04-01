using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinClever.Models.invest;
using FinClever.Repositories;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace FinClever
{
    [DisallowConcurrentExecution]
    public class CachingStockHistoryJob : IJob
    {
        IPortfolioRepository portfolioRepository;
        IStockRepository stockRepository;

        public CachingStockHistoryJob(IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("CachingStockHistoryJob start");
            var allTickers = await portfolioRepository.GetAllTickets();
            var prices = new List<StockPriceCache>();

            foreach (var t in allTickers)
            {
                Console.WriteLine($"Ticker: {t}");
                var history = await stockRepository.GetStockHistory(t);
                if (history?.Prices != null)
                {
                    Console.WriteLine($"History size: {history.Prices.Count}");
                    prices.AddRange(history.Prices.Select(p =>
                    {
                        var date = ParseDate(p.Key);
                        var price = double.Parse(p.Value.Close ?? "0");
                        return new StockPriceCache(date, t, price);
                    }));
                }
            }
            var updatedPrices = await stockRepository.SavePriceHistoryCache(prices);
            Console.WriteLine($"Updated prices: {updatedPrices}");
            Console.WriteLine("CachingStockHistoryJob end");
        }

        private long ParseDate(string date)
        {
            return ((DateTimeOffset)DateTime.Parse(date)).ToUnixTimeSeconds();
        }
    }
}
