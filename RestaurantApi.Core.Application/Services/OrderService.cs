using AutoMapper;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels;
using RestaurantApi.Core.Application.ViewModels.Dish;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Services
{
    public class OrderService : GenericService<OrderViewModel, Order> , IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepo;
        private readonly IOrderRepository _repository;

        public OrderService(IGenericRepository<Order> repository, IMapper mapper, IDishRepository dishRepository
            , IOrderRepository orderRepository) : base(repository, mapper) { 
            _mapper = mapper;
            _dishRepo = dishRepository;
            _repository = orderRepository;
        }

        public async Task<Order> AddService(ListOrderViewModel vm, List<int> dishes)
        {
            Order dish = _mapper.Map<Order>(vm);
            List<Dish> dishs = await _dishRepo.GetAllWithInclude();
            List<Dish> orderDishes = [];
            double subtotal = 0;

            foreach (var item in dishes)
            {
                var plato = await _dishRepo.GetById(item);
                if (plato != null)
                {
                    orderDishes.Add(plato);
                    subtotal += plato.Price;
                }
            }

            dish.Subtotal = subtotal;
            dish.State = "En Proceso";

            dish = await _repository.Add(dish, orderDishes);

            return dish;
        }

        public async Task<List<OrderViewModel>> GetAllWithInclude()
        {
            var list = await _repository.GetAllWithInclude();

            var result = _mapper.Map<List<OrderViewModel>>(list);

            return result;
        }

        public async Task<OrderViewModel> GetByIdWithInclude(int id)
        {
            var result = await _repository.GetByIdWithInclude(id);
            var orderVM = _mapper.Map<OrderViewModel>(result);

            return orderVM;
        }

        public async Task UpdateService(UpdateOrderViewModel vm, int id)
        {
            Order entity = _mapper.Map<Order>(vm);

            List<Dish> orderDishes = [];

            foreach (var item in vm.DishesIds)
            {
                var dish = await _dishRepo.GetById(item);
                if (dish != null)
                {
                    orderDishes.Add(dish);
                }
            }

            await _repository.Update(entity, id, orderDishes);
        }
    }
}
