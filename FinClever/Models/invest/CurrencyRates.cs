using System;
using Newtonsoft.Json;

namespace FinClever.Models
{
    public class CurrencyRates
    {
        public CurrencyRatesData Data { get; set; }
    }

    public class CurrencyRatesQuery
    {
        [JsonProperty(PropertyName = "apikey")]
        public string ApiKey { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty(PropertyName = "base_currency")]
        public string BaseCurrency { get; set; }
    }

    public class CurrencyRatesData
    {
        [JsonProperty(PropertyName = "RUB")]
        public double Rub { get; set; }
    }
}
