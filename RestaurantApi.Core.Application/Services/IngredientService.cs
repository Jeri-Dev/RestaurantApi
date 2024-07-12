using AutoMapper;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Ingredients;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Services
{
    public class IngredientService : GenericService<IngredientViewModel, Ingredient> , IIngredientService
    {
        private readonly IIngredientRepository _repository;
        private readonly IMapper _mapper;
        public IngredientService(IGenericRepository<Ingredient> repository, IMapper mapper, IIngredientRepository repo) : base(repository, mapper) {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task UpdateService(UpdateIngredientViewModel vm, int id)
        {
            Ingredient entity = _mapper.Map<Ingredient>(vm);
            await _repository.Update(entity, id);
        }

    }

}
