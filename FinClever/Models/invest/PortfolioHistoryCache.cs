using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinClever.Models.invest
{
	public class PortfolioHistoryCache
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long Date { get; set; }
		public double Price { get; set; }
        public int ShowHistoricalProfit { get; set; }
        public string Range { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public PortfolioHistoryCache(int id, long date, double price, int showHistoricalProfit, string range, string userId)
        {
            Id = id;
            Date = date;
            Price = price;
            ShowHistoricalProfit = showHistoricalProfit;
            Range = range;
            UserId = userId;
        }
    }
}

