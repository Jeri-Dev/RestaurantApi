using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.Services;

namespace RestaurantApi.Core.Application
{
public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Services DI
        services.AddTransient(typeof(IGenericService<,>), typeof(GenericService<,>));
        services.AddTransient<IDishService, DishService>();
        services.AddTransient<IIngredientService, IngredientService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ITableService, TableService>();
    }
    }
}