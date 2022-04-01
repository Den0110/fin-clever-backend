using System;
using Newtonsoft.Json;

namespace FinClever.Models
{
    public class FinnhubQuote
    {
        [JsonProperty(PropertyName = "c")]
        public double CurrentPrice { get; set; }
    }
}
