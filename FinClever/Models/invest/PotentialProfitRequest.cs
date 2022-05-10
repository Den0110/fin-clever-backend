using System;
namespace FinClever.Models.invest
{
    public class PotentialProfitRequest
    {
        public long Date { get; set; }
        public double Sum { get; set; }

        public PotentialProfitRequest(long date, double sum)
        {
            Date = date;
            Sum = sum;
        }
    }
}
