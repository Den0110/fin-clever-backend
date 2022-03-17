using System;
using System.Threading.Tasks;

namespace FinClever.Repositories
{
    public interface ICurrencyRepository
    {
        Task<double> GetUsdRate(DateTime? date = null);
    }
}
