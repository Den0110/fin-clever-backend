using System.Collections.Generic;
using System.Threading.Tasks;
using SimplePatch;

namespace FinClever.Repositories
{
    public interface IAccountRepository
    {
        Task<List<Account>> Get();
        Task<Account> Get(int id);
        Task<Account> Create(Account account);
        Task Update(Account account);
        Task Delete(int id);
    }
}
