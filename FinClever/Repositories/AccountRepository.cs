using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplePatch;

namespace FinClever.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinCleverDbContext context;

        public AccountRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<Account> Create(Account account)
        {
            context.Accounts.Add(account);
            await context.SaveChangesAsync();
            return account;
        }

        public async Task Delete(int id)
        {
            var account = await context.Accounts.FindAsync(id);
            context.Accounts.Remove(account);
            await context.SaveChangesAsync();
        }

        public async Task<List<Account>> Get()
        {
            return await context.Accounts.ToListAsync();
        }

        public async Task<Account> Get(int id)
        {
            return await context.Accounts.FindAsync(id);
        }

        public async Task Update(Account account)
        {
            context.Entry(account).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
