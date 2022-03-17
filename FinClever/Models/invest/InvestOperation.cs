using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FinClever.Models
{
    public class InvestOperation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long Date { get; set; }
        public string Ticker { get; set; }
        public double Price { get; set; }
        public double UsdPrice { get; set; }
        public int Amount { get; set; }

        [JsonIgnore]
        [ForeignKey("User")]
        public string? UserId { get; set; }
    }
}
