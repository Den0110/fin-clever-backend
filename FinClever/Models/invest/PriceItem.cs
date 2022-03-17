using System;
namespace FinClever.Models.invest
{
    public class PriceItem
    {
        public long Date { get; set; }
        public double Price { get; set; }

        public PriceItem(long date, double price)
        {
            Date = date;
            Price = price;
        }
    }
}
