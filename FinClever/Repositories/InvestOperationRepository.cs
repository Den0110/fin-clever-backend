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

        public async Task Delete(int id)
        {
            var operation = await context.InvestOperations.FindAsync(id);
            context.InvestOperations.Remove(operation);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvestOperation>> Get()
        {
            return await context.InvestOperations.ToListAsync();
        }

        public async Task<InvestOperation> Get(int id)
        {
            return await context.InvestOperations.FindAsync(id);
        }

        public async Task<IEnumerable<InvestOperation>> GetForTicker(string ticker)
        {
            return await context.InvestOperations.Where(x => x.Ticker == ticker).ToListAsync();
        }

        public async Task Update(InvestOperation operation)
        {
            context.Entry(operation).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
