using ImportEngine.Features.Buyers.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportEngine.Features.Buyers.Models
{
    public class BuyerDbContext : DbContext
    {
        public BuyerDbContext(DbContextOptions<BuyerDbContext> options) : base(options)
        {
        }

        public DbSet<Buyer> Buyers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicit configuration taake EF Core ko pta ho ke primary key aur table kya hai
            modelBuilder.Entity<Buyer>().ToTable("Buyers");
            modelBuilder.Entity<Buyer>().HasKey(b => b.Id);
        }
    }
}
