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
            var allTickers = await portfolioRepository.GetAllTickets();
            var prices = new List<StockPriceCache>();

            foreach (var t in allTickers)
            {
                var history = await stockRepository.GetStockHistory(t);
                prices.AddRange(history.Select(x =>
                {
                    var date = ParseDate(x.Date);
                    var price = x.Close;
                    return new StockPriceCache(date, t, price);
                }));
            }
            await stockRepository.SavePriceHistoryCache(prices);
        }

        private long ParseDate(string date)
        {
            return ((DateTimeOffset)DateTime.Parse(date)).ToUnixTimeSeconds();
        }
    }
}
