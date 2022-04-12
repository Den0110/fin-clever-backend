using FinClever.Controllers;
using FinClever.Models;
using FinClever.Models.invest;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinClever
{
    public class FinCleverDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public FinCleverDbContext(DbContextOptions options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<InvestOperation> InvestOperations { get; set; }

        public DbSet<HistoryStockPriceCache> HistoryStockPrices { get; set; }

        public DbSet<CurrentStockPriceCache> CurrentStockPrices { get; set; }

        public DbSet<PortfolioHistoryCache> PortfolioHistoryCache { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Account>()
                .HasQueryFilter(account => account.UserId == _httpContextAccessor.HttpContext.User.GetId());
            builder.Entity<Operation>()
                .HasQueryFilter(operation => operation.UserId == _httpContextAccessor.HttpContext.User.GetId());
        }


    }
}
