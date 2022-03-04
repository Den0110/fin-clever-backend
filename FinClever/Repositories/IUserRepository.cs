using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinClever.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();
        Task<User> Get(string id);
        Task<User> Create(User user);
        Task Update(User user);
        Task Delete(string id);
    }
}
