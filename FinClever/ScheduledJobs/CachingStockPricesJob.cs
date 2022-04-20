using System;
using System.Threading.Tasks;
using FinClever.Models.invest;
using FinClever.Repositories;
using Quartz;

namespace FinClever
{
    [DisallowConcurrentExecution]
    public class CachingStockPricesJob : IJob
    {
        IPortfolioRepository portfolioRepository;
        IStockRepository stockRepository;

        public CachingStockPricesJob(IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.stockRepository = stockRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Prices caching start");

            var allTickers = await portfolioRepository.GetAllTickets();
            foreach(var ticker in allTickers)
            {
                var price = (await stockRepository.GetStock(ticker))?.CurrentPrice ?? .0;
                var date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                var priceCache = new CurrentStockPriceCache(date, ticker, price);
                await stockRepository.UpdateCurrentPriceCache(priceCache);
            }

            Console.WriteLine("Prices caching end");
        }
    }
}

