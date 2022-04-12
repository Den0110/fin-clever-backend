using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinClever.Models.invest
{
    public class CurrentStockPriceCache
    {
        [Key]
        public string Ticker { get; set; }
        public long Date { get; set; }
        public double Price { get; set; }

        public CurrentStockPriceCache(long date, string ticker, double price)
        {
            Date = date;
            Ticker = ticker;
            Price = price;
        }
    }
}
