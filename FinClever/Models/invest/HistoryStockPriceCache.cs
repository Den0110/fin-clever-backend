using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Models.invest
{
    public class HistoryStockPriceCache
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long Date { get; set; }
        public string Ticker { get; set; }
        public double Price { get; set; }

        public HistoryStockPriceCache(long date, string ticker, double price)
        {
            Date = date;
            Ticker = ticker;
            Price = price;
        }
    }
}
