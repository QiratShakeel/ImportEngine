using ImportEngine.Features.Buyers.Repositories;
using ImportEngine.Features.Buyers.Interfaces;
using ImportEngine.Features.Buyers.Models;
using Microsoft.EntityFrameworkCore;

namespace EasternGarments.ERP.ImportEngine.Features.Buyers
{
public static class BuyerModuleExtensions
{
    public static IServiceCollection AddBuyerModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BuyerDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IBuyerRepository, BuyerRepository>();
        return services;
    }
}
}