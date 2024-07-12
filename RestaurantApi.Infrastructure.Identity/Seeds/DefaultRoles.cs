using RestaurantApi.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace LazaRestaurant.Infrastructure.Identity.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Mesero.ToString()));
    }
}