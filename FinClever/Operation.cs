using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever
{
    public class Operation
    {
        public int Id { get; set; }

        public float value { get; set; }

        public string type { get; set; }

        public string category { get; set; }

        public int accountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
