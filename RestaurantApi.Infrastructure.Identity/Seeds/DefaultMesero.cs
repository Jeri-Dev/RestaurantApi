using RestaurantApi.Core.Application.Enums;
using RestaurantApi.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace RestaurantApi.Infrastructure.Identity.Seeds;

public static class DefaultMesero
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        ApplicationUser defaultServer = new ApplicationUser()
        {
            UserName = "mesero",
            Email = "defaultemesero@gmail.com",
            FirstName = "default",
            LastName = "Mesero",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultServer.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultServer.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultServer, "My_P4ssw0rd");
                await userManager.AddToRoleAsync(defaultServer, Roles.Mesero.ToString());
            }
        }
    }
}