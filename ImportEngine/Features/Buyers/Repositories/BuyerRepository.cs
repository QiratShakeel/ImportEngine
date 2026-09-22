using Microsoft.EntityFrameworkCore;
using ImportEngine.Features.Buyers.Interfaces;
using ImportEngine.Features.Buyers.Models;

namespace ImportEngine.Features.Buyers.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly BuyerDbContext _context;

        public BuyerRepository(BuyerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Buyer>> GetActiveBuyersAsync()
        {
            // Read-only operation ke liye AsNoTracking memory save karta hai
            return await _context.Buyers
                .AsNoTracking()
                .Select(b => new Buyer
                {
                    Id = b.Id,
                    BuyerName = b.BuyerName
                    // Dropdown ke liye target table name ko fetch karne ki zaroorat nahi hai, memory optimize rkhni ha
                })
                .ToListAsync();
        }

        public async Task<string> GetTargetTableNameAsync(int buyerId)
        {
            var targetTable = await _context.Buyers
                .AsNoTracking()
                .Where(b => b.Id == buyerId)
                .Select(b => b.TargetTableName)
                .FirstOrDefaultAsync();

            return targetTable ?? string.Empty;
        }
    }
}
