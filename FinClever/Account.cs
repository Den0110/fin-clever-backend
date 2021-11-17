using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
