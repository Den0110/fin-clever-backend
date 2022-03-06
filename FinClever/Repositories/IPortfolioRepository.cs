using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;

namespace FinClever.Repositories
{
    public interface IPortfolioRepository
    {
        Task<IEnumerable<PortfolioStock>> GetStocks();
    }
}
