using AutoMapper;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Dish;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Services
{
    public class DishService : GenericService<DishViewModel, Dish> , IDishService
    {
        private readonly IDishRepository _repository;
        private readonly IMapper _mapper;
        private readonly IIngredientRepository _ingredientRepo;
        public DishService(IGenericRepository<Dish> repository, IMapper mapper, IDishRepository repo, 
            IIngredientRepository ingredientRepo) : base(repository, mapper)
        {
            _repository = repo;
            _mapper = mapper;
            _ingredientRepo = ingredientRepo;
        }

        public async Task<Dish> AddService(ListDishViewModel vm, List<int> ingredientes)
        {
            Dish dish = _mapper.Map<Dish>(vm);
            List<Ingredient> ingredients = await _ingredientRepo.GetAll();
            List<Ingredient> dishIngredients = [];

            foreach (var item in ingredientes)
            {
                var ingrediente = await _ingredientRepo.GetById(item);
                if (ingrediente != null) { 
                    dishIngredients.Add(ingrediente);
                }
            }

            dish = await _repository.Add(dish, dishIngredients);

            return dish;
        }

        public async Task<List<DishViewModel>> GetAllWithInclude()
        {
            var list = await _repository.GetAllWithInclude();

            var result = _mapper.Map<List<DishViewModel>>(list);

            return result;
        }

        public async Task<DishViewModel> GetByIdWithInclude(int id)
        {
            var result = await _repository.GetByIdWithInclude(id);
            var dishVM = _mapper.Map<DishViewModel>(result);

            return dishVM;
        }

        public async Task UpdateService(UpdateDishViewModel vm, int id)
        {
            Dish entity = _mapper.Map<Dish>(vm);

            List<Ingredient> dishIngredients = [];

            foreach (var item in vm.IngredientsIds)
            {
                var ingrediente = await _ingredientRepo.GetById(item);
                if (ingrediente != null)
                {
                    dishIngredients.Add(ingrediente);
                }
            }

            await _repository.Update(entity, id, dishIngredients);
        }
    }
}
