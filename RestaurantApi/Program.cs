using RestaurantApi.Infrastructure.Persistence;
using System.Text.Json.Serialization;
using RestaurantApi.Core.Application;
using RestaurantApi.Presentation.WebApi.Extensions;
using RestaurantApi.Infrastructure.Identity;
using LazaRestaurant.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using RestaurantApi.Infrastructure.Identity.Contexts;
using RestaurantApi.Infrastructure.Identity.Entities;
using RestaurantApi.Infrastructure.Identity.Seeds;
using RestaurantApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();
builder.Services.AddApplicationLayer();
builder.Services.AddIdentityLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    try
    {
       
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(roleManager);
        await DefaultAdmin.SeedAsync(userManager);
        await DefaultMesero.SeedAsync(userManager);
        await DefaultSuperAdmin.SeedAsync(userManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.MapControllers();

app.Run();