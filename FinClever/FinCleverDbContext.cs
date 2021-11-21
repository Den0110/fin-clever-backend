using Microsoft.EntityFrameworkCore;

namespace FinClever
{
    public class FinCleverDbContext : DbContext
    {
        public FinCleverDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Operation> Operations { get; set; }

    }
}
