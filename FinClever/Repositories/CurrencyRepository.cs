using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using FinClever.Models;

namespace FinClever.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        static string CurrencyUrl = "https://freecurrencyapi.net/api/v2/latest?apikey=41a4d050-9da0-11ec-8603-ab535655507b";
        static HttpClient httpClient = new HttpClient();

        public async Task<double> GetUsdRate(DateTime? date)
        {
            double price = .0;
            var response = await httpClient.GetAsync(CurrencyUrl);
            if (response.IsSuccessStatusCode)
            {
                var rates = await response.Content.ReadAsAsync<CurrencyRates>();
                price = rates.Data.Rub;
            }
            return price;
        }
    }
}
