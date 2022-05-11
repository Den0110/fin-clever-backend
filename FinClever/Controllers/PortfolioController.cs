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
        private readonly SpyPricesCache spyPricesCache;

        public PortfolioController(IPortfolioRepository portfolioRepository, IStockRepository stockRepository, SpyPricesCache spyPricesCache)
        {
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
            this.spyPricesCache = spyPricesCache;
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
                if (prices.ContainsKey(s.Ticker))
                {
                    s.CurrentPrice = prices[s.Ticker];
                }
                else
                {
                    var stock = await stockRepository.GetStock(s.Ticker);
                    s.CurrentPrice = stock?.CurrentPrice ?? .0;
                }
                s.CompanyName = s.Ticker;
                totalPrice += s.CurrentPrice * s.Amount ?? .0;
            }

            var priceHistory = await portfolioRepository.GetPortfolioHistory(
                User.GetId(),
                TimeUtils.GetDaysForRange(range ?? "M"),
                showHistoricalProfit ?? false
            );

            priceHistory.Last().Price = totalPrice;

            return new Portfolio(totalPrice, priceHistory, stocks);
        }

        [HttpPost]
        [Route("potentialProfit")]
        public async Task<ActionResult<double>> GetPotentialProfit([FromBody] PotentialProfitRequest request)
        {
            var minDelta = long.MaxValue;
            var dateForPortfolio = 0L;
            TimeUtils.GetDaysForRange("ALL").ToList().ForEach(x => {
                var delta = Math.Abs(x - request.Date);
                if (delta < minDelta)
                {
                    dateForPortfolio = x;
                    minDelta = delta;
                }
            });

            var startAndEnd = await portfolioRepository.GetPortfolioHistory(
                User.GetId(),
                new[] { dateForPortfolio, ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() },
                true
            );
            if (startAndEnd.Count() < 2 || startAndEnd.Last().Price == 0 || startAndEnd.First().Price == 0)
            {
                if (spyPricesCache.History == null ||
                    Math.Abs(TimeUtils.ParseDate(spyPricesCache.History.Prices?.Last().Key ?? "0") - TimeUtils.GetTime()) > 86400)
                {
                    spyPricesCache.History = await stockRepository.GetStockHistory("SPY");
                }
                if (spyPricesCache.History?.Prices == null || spyPricesCache.History.Prices.LongCount() == 0)
                    return BadRequest();
                var startPrice = .0;
                minDelta = long.MaxValue;
                spyPricesCache!.History.Prices.ToList().ForEach(x => {
                    var delta = Math.Abs(TimeUtils.ParseDate(x.Key) - request.Date);
                    if (delta < minDelta)
                    {
                        startPrice = double.Parse(x.Value.Close ?? "0");
                        minDelta = delta;
                    }
                });
                var endPrice = double.Parse(spyPricesCache.History.Prices?.Last().Value.Close ?? "0");
                if (startPrice == 0 || endPrice == 0)
                    return BadRequest();
                if(endPrice <= startPrice)
                    return BadRequest();
                return request.Sum * (endPrice / startPrice);
            }
            if (startAndEnd.Last().Price <= startAndEnd.First().Price)
                return BadRequest();
            return request.Sum * (startAndEnd.Last().Price / startAndEnd.First().Price);
        }
    }
}
