using System.Collections.Generic;
using System.Threading.Tasks;
using FinClever.Models;

namespace FinClever.Repositories
{
    public interface IInvestOperationRepository
    {
        Task<IEnumerable<InvestOperation>> Get();
        Task<IEnumerable<InvestOperation>> GetForTicker(string ticker);
        Task<InvestOperation> Get(int id);
        Task<InvestOperation> Create(InvestOperation operation);
        Task Update(InvestOperation operation);
        Task Delete(int id);
    }
}
