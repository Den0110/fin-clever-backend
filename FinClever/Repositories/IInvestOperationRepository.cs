using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;

namespace FinClever.Repositories
{
    public interface IInvestOperationRepository
    {
        Task<IEnumerable<InvestOperation>> Get(string userId);
        Task<IEnumerable<InvestOperation>> GetForTicker(string userId, string ticker);
        Task<InvestOperation> Get(string userId, int id);
        Task<InvestOperation> Create(InvestOperation operation);
        Task Update(InvestOperation operation);
        Task Delete(string userId, int id);
        Task<bool> HasOne(string userId);
    }
}
