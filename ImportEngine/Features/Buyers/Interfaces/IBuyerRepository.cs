using ImportEngine.Features.Buyers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportEngine.Features.Buyers.Interfaces
{
    public interface IBuyerRepository
    {
        // 1. Dropdown karne ke liye list fetch karega
        Task<IEnumerable<Buyer>> GetActiveBuyersAsync();

        // 2. Selected buyer ka target table configuration lookup karega [O(1)]
        Task<string> GetTargetTableNameAsync(int buyerId);
    }
}
