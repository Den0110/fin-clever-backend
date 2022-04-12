using System.ComponentModel.DataAnnotations.Schema;

namespace FinClever.Models.invest
{
    public class CurrentStockPriceCache
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long Date { get; set; }
        public string Ticker { get; set; }
        public double Price { get; set; }

        public CurrentStockPriceCache(long date, string ticker, double price)
        {
            Date = date;
            Ticker = ticker;
            Price = price;
        }
    }
}
