using System;
using System.Threading.Tasks;
using FinClever.Repositories;
using Quartz;

namespace FinClever
{
    [DisallowConcurrentExecution]
    public class CachingPortfolioHistoryJob : IJob
    {
        IPortfolioRepository portfolioRepository;

        public CachingPortfolioHistoryJob(IPortfolioRepository portfolioRepository)
        {
            this.portfolioRepository = portfolioRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Portfolio caching start");

            await UpdateHistoryFor("W", "1Y", "ALL");

            Console.WriteLine("Portfolio caching end");
        }

        private async Task UpdateHistoryFor(params string[] ranges)
        {
            foreach(var range in ranges)
            {
                var days = TimeUtils.GetDaysForRange(range);
                await portfolioRepository.UpdatePortfoliosCache(days, range);
            }
        }
    }
}

