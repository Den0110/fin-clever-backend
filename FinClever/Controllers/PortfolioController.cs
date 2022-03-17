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
        public async Task<Portfolio> GetStocks()
        {
            var totalPrice = .0;
            var stocks = await portfolioRepository.GetStocks();

            foreach (var s in stocks)
            {
                var ticker = s.Ticker;
                var stockInfo = await stockRepository.GetStock(ticker);
                s.CurrentPrice = stockInfo.LatestPrice;
                s.CompanyName = stockInfo.CompanyName;
                totalPrice += s.CurrentPrice * s.Amount ?? .0;
            }

            var priceHistory = new List<PriceItem>();

            foreach (var day in GetDaysFor6m())
            {
                var dayStocks = await portfolioRepository.GetStocks(day);
                var portfolioPrice = .0;
                foreach (var stock in dayStocks)
                {
                    var item = await stockRepository.GetPriceHistoryCache(day, stock.Ticker);
                    portfolioPrice += item.Price * stock.Amount;
                }
                priceHistory.Add(new PriceItem(day, portfolioPrice));
            }

            return new Portfolio(totalPrice, priceHistory, stocks);
        }

        private static IEnumerable<long> GetDaysFor6m()
        {
            var startDay = ((DateTimeOffset) DateTime.Now.Date.AddMonths(-6)).ToUnixTimeSeconds();
            var endDay = ((DateTimeOffset)DateTime.Now.Date).ToUnixTimeSeconds();
            var step = 24 * 60 * 60;
            for (long i = startDay; i <= endDay; i += step)
                yield return i;
        }

    }
}
