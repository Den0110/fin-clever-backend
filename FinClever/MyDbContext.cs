using System;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Entity;

namespace FinClever
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DbConnectionString")
        {   
        }

        public DbSet<Table1> Table { get; set; }
    }
}
