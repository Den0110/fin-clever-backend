using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinClever.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private readonly FinCleverDbContext context;

        public OperationRepository(FinCleverDbContext context)
        {
            this.context = context;
        }

        public async Task<Operation> Create(Operation operation)
        {
            context.Operations.Add(operation);
            await context.SaveChangesAsync();
            return operation;
        }

        public async Task Delete(int id)
        {
            var operation = await context.Operations.FindAsync(id);
            context.Operations.Remove(operation);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Operation>> Get()
        {
            return await context.Operations.ToListAsync();
        }

        public async Task<Operation> Get(int id)
        {
            return await context.Operations.FindAsync(id);
        }

        public async Task Update(Operation operation)
        {
            context.Entry(operation).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
