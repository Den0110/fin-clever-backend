﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinClever.Models.invest;
using FinClever.Repositories;
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
            Console.WriteLine("History caching start");
            var allTickers = await portfolioRepository.GetAllTickets();
            var prices = new List<HistoryStockPriceCache>();

            var counter = 0;
            foreach (var t in allTickers)
            {
                // pause for 2 min every 5 requests not to break the API limit
                if (counter >= 5)
                {
                    Thread.Sleep(120000);
                    counter = 0;
                }
                Console.WriteLine($"Ticker: {t}");
                var history = await stockRepository.GetStockHistory(t);
                if (history?.Prices != null)
                {
                    Console.WriteLine($"History size: {history.Prices.Count}");
                    prices.AddRange(history.Prices.Select(p =>
                    {
                        var date = ParseDate(p.Key);
                        var price = double.Parse(p.Value.Close ?? "0");
                        return new HistoryStockPriceCache(date, t, price);
                    }));
                }
                counter++;
            }
            var updatedPrices = await stockRepository.SavePriceHistoryCache(prices);
            Console.WriteLine($"Updated prices: {updatedPrices}");
            Console.WriteLine("History caching end");
        }

        private long ParseDate(string date)
        {
            return ((DateTimeOffset)DateTime.Parse(date)).ToUnixTimeSeconds();
        }
    }
}
