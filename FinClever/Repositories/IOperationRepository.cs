using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinClever.Repositories
{
    public interface IOperationRepository
    {
        Task<IEnumerable<Operation>> Get();
        Task<Operation> Get(int id);
        Task<Operation> Create(Operation operation);
        Task Update(Operation operation);
        Task Delete(int id);
    }
}
