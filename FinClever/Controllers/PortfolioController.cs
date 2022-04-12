using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;
using FinClever.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinClever.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/invest/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IStockRepository stockRepository;

        public PortfolioController(IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<Portfolio> GetStocks(string? range = null, bool? showHistoricalProfit = null)
        {
            var totalPrice = .0;
            var stocks = await portfolioRepository.GetStocks(User.GetId());
            var prices = (await stockRepository.GetCurrentPriceCache())
                .ToDictionary(x => x.Ticker, x => x.Price);

            foreach (var s in stocks)
            {
                if(prices.ContainsKey(s.Ticker)) {
                    s.CurrentPrice = prices[s.Ticker];
                    s.CompanyName = s.Ticker;
                    totalPrice += s.CurrentPrice * s.Amount ?? .0;
                }
            }

            var priceHistory = await portfolioRepository.GetPortfolioHistory(User.GetId(), GetDaysForRange(range ?? "M"), showHistoricalProfit ?? false);

            priceHistory.Last().Price = totalPrice;

            return new Portfolio(totalPrice, priceHistory, stocks);
        }

        private static long GetTime()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
        }

        private static IEnumerable<long> GetDaysForRange(string range)
        {
            var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var startDate = endDate.AddMonths(-1);
            var dayStep = 1;
            switch (range)
            {
                case "W":
                    startDate = endDate.AddDays(-7);
                    dayStep = 1;
                    break;
                case "M":
                    startDate = endDate.AddMonths(-1);
                    dayStep = 1;
                    break;
                case "6M":
                    startDate = endDate.AddMonths(-6);
                    dayStep = 7;
                    break;
                case "1Y":
                    startDate = endDate.AddYears(-1);
                    dayStep = 7;
                    break;
                case "ALL":
                    startDate = endDate.AddYears(-20);
                    dayStep = 6 * 30;
                    break;
            }
            
            var startDay = ((DateTimeOffset)startDate).ToUnixTimeSeconds();
            var endDay = ((DateTimeOffset)endDate).ToUnixTimeSeconds();
            var step = dayStep * 24 * 60 * 60;
            long i;
            for (i = startDay; i <= endDay; i += step)
                yield return i;
            if (i != endDay + step)
                yield return endDay;
        }

    }
}
