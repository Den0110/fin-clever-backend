
namespace FinClever.Models
{
    public class PortfolioStock
    {
        public string Ticker { get; set; }
        public double UsdPurchasePrice { get; set; }
        public double PurchasePrice { get; set; }
        public double? CurrentPrice { get; set; }
        public int Amount { get; set; }

        public PortfolioStock(string ticker, double usdPurchasePrice, double purchasePrice, int amount)
        {
            Ticker = ticker;
            UsdPurchasePrice = usdPurchasePrice;
            PurchasePrice = purchasePrice;
            Amount = amount;
        }
    }
}
