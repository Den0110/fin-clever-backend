﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;
using FinClever.Models.invest;

namespace FinClever.Repositories
{
    public interface IPortfolioRepository
    {
        Task<IEnumerable<string>> GetAllTickets();
        Task<IEnumerable<PortfolioStock>> GetStocks(string userId, long? date = null);
        Task<IEnumerable<PriceItem>> GetPortfolioHistory(string userId, IEnumerable<long> dates, bool showHistoricalProfit = false);
    }
}
