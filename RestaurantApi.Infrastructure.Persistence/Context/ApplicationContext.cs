using RestaurantApi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApi.Infrastructure.Persistence.Contexts;

public class ApplicationContext : DbContext
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<DishIngredient> DishIngredients { get; set; }
    public DbSet<OrderDish> OrderDishes { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Tables

        modelBuilder.Entity<Dish>()
            .ToTable("Dishes");
        
        modelBuilder.Entity<Ingredient>()
            .ToTable("Ingredients");
        
        modelBuilder.Entity<Order>()
            .ToTable("Orders");
        
        modelBuilder.Entity<Table>()
            .ToTable("Tables");    
        
        modelBuilder.Entity<DishIngredient>()
            .ToTable("DishIngredients");
        
        modelBuilder.Entity<OrderDish>()
            .ToTable("OrderDishes");

        #endregion
        
        #region Primary Keys

        modelBuilder.Entity<Dish>()
            .HasKey(dish => dish.Id);
        
        modelBuilder.Entity<Ingredient>()
            .HasKey(ingredient => ingredient.Id);
        
        modelBuilder.Entity<Order>()
            .HasKey(order => order.Id);
        
        modelBuilder.Entity<Table>()
            .HasKey(table => table.Id);

        modelBuilder.Entity<DishIngredient>()
            .HasKey(dishIngredient => new { dishIngredient.DishId, dishIngredient.IngredientId });

        modelBuilder.Entity<OrderDish>()
            .HasKey(dishOrder => new { dishOrder.OrderId, dishOrder.DishId });

        #endregion
        
        #region Relationships
        
        modelBuilder.Entity<Ingredient>()
            .HasMany<DishIngredient>(ingredient => ingredient.DishIngredients)
            .WithOne(dishIngredient => dishIngredient.Ingredient)
            .HasForeignKey(dishIngredient => dishIngredient.IngredientId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<Dish>()
            .HasMany<DishIngredient>(dish => dish.DishIngredients)
            .WithOne(dishIngredient => dishIngredient.Dish)
            .HasForeignKey(dishIngredient => dishIngredient.DishId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        modelBuilder.Entity<Dish>()
            .HasMany<OrderDish>(dish => dish.OrderDishes)
            .WithOne(orderDish => orderDish.Dish)
            .HasForeignKey(orderDish => orderDish.DishId)
            .OnDelete(DeleteBehavior.ClientCascade);        
        
        modelBuilder.Entity<Order>()
            .HasMany<OrderDish>(order => order.OrderDishes)
            .WithOne(orderDish => orderDish.Order)
            .HasForeignKey(orderDish => orderDish.OrderId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        modelBuilder.Entity<Table>()
            .HasMany<Order>(table => table.Orders)
            .WithOne(order => order.Table)
            .HasForeignKey(order => order.TableId)
            .OnDelete(DeleteBehavior.Cascade);
        
        #endregion
        
    }
}