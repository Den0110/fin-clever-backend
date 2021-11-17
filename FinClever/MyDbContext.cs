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

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Operation> Operations { get; set; }
    }
}
