using RestaurantApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Infrastructure.Persistence.Repositories;

namespace RestaurantApi.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        
        // Contexts
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));

        });

        // Repositories Registration
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IDishRepository, DishRepository>();
        services.AddTransient<IIngredientRepository, IngredientRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<ITableRepository, TableRepository>();
    }
}