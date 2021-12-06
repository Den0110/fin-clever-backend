using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinClever
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Balance { get; set; }
    }
}
