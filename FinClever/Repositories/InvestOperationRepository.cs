using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinClever.Models;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class InvestOperationRepository : IInvestOperationRepository
    {
        private readonly FinCleverDbContext context;

        public InvestOperationRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<InvestOperation> Create(InvestOperation operation)
        {
            context.InvestOperations.Add(operation);
            await context.SaveChangesAsync();
            return operation;
        }

        public async Task Delete(string userId, int id)
        {
            var operation = await context.InvestOperations.FirstAsync(x => x.UserId == userId && x.Id == id);
            context.InvestOperations.Remove(operation);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvestOperation>> Get(string userId)
        {
            return await context.InvestOperations.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<InvestOperation> Get(string userId, int id)
        {
            return await context.InvestOperations.FirstAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<IEnumerable<InvestOperation>> GetForTicker(string userId, string ticker)
        {
            return await context.InvestOperations.Where(x => x.UserId == userId && x.Ticker == ticker).ToListAsync();
        }

        public async Task Update(InvestOperation operation)
        {
            context.Entry(operation).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<bool> HasOne(string userId)
        {
            return await context.InvestOperations.Where(x => x.UserId == userId).CountAsync() > 0;
        }
    }
}
