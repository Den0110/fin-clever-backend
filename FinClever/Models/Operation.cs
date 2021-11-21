using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever
{
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public float Value { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
