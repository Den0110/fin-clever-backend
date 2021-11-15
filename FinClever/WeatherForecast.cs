using System;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Entity;

namespace FinClever
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
