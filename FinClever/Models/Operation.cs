using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Azure;

namespace FinClever
{
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public float Value { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public long Date { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public float AbsoluteBalanceEffect()
        {
            switch (Type)
            {
                case "expense": return -Value;
                case "income": return Value;
                default: return 0;
            }
        }
    }
}
