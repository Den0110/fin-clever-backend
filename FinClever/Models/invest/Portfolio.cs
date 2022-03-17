using System;
using System.Collections.Generic;
using FinClever.Models.invest;

namespace FinClever.Models
{
    public struct Portfolio
    {
        public double TotalPrice { get; set; }
        public IEnumerable<PriceItem> PriceHistory { get; set; }
        public IEnumerable<PortfolioStock> Stocks { get; set; }

        public Portfolio(double totalPrice, IEnumerable<PriceItem> priceHistory, IEnumerable<PortfolioStock> stocks)
        {
            TotalPrice = totalPrice;
            PriceHistory = priceHistory;
            Stocks = stocks;
        }
    }
}
