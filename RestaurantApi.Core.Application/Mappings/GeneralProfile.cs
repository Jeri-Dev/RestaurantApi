using AutoMapper;
using RestaurantApi.Core.Application.ViewModels.Dish;
using RestaurantApi.Core.Application.ViewModels.Ingredients;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Application.ViewModels.Table;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // Dishes
        CreateMap<Dish, DishViewModel>()
            .ReverseMap();
        CreateMap<Dish, ListDishViewModel>()
            .ReverseMap();
        CreateMap<Dish, UpdateDishViewModel>()
           .ReverseMap();

        // Ingredients
        CreateMap<Ingredient, IngredientViewModel>()
            .ReverseMap()
           .ForMember(dest => dest.DishIngredients, opt => opt.Ignore());

        CreateMap<Ingredient, UpdateIngredientViewModel>()
           .ReverseMap();

        CreateMap<IngredientViewModel, UpdateIngredientViewModel>()
           .ReverseMap()
           .ForMember(dest => dest.Id, opt => opt.Ignore());


        // Orders
        CreateMap<Order, OrderViewModel>()
            .ReverseMap();

        CreateMap<Order, ListOrderViewModel>()
            .ReverseMap();

        CreateMap<Order, UpdateOrderViewModel>()
            .ReverseMap();

        // Tables
        CreateMap<Table, TableViewModel>()
            .ReverseMap();
        CreateMap<Table, ListTableViewModel>()
            .ReverseMap();
        CreateMap<Table, UpdateTableViewModel>()
            .ReverseMap();
        CreateMap<Table, ChangeStatusTableViewModel>()
            .ReverseMap();
    }
}