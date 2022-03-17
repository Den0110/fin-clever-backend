using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinClever.Models;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly FinCleverDbContext context;

        public PortfolioRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PortfolioStock>> GetStocks(long? date = null)
        {
            if(date == null)
            {
                date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            }
            return await context.InvestOperations
                .Where(o => o.Date <= date)
                .GroupBy(o => o.Ticker)
                .Select(g => new PortfolioStock(
                    g.Key,
                    g.Sum(x => x.UsdPrice * x.Amount) / g.Sum(x => x.Amount),
                    g.Sum(x => x.Price * x.Amount) / g.Sum(x => x.Amount),
                    g.Sum(x => x.Amount)
                ))
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllTickets()
        {
            return await context.InvestOperations
                .GroupBy(o => o.Ticker)
                .Select(x => x.Key)
                .ToListAsync();
        }
    }
}
