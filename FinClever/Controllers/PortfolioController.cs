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

            var priceHistory = await portfolioRepository.GetPortfolioHistory(
                User.GetId(),
                TimeUtils.GetDaysForRange(range ?? "M"),
                showHistoricalProfit ?? false
            );

            priceHistory.Last().Price = totalPrice;

            return new Portfolio(totalPrice, priceHistory, stocks);
        }

    }
}
