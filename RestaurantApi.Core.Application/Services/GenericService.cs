using AutoMapper;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Application.Interfaces.Services;

namespace RestaurantApi.Core.Application.Services
{
    public class GenericService<ViewModel, T> : IGenericService<ViewModel, T>
        where ViewModel : class
        where T : class
    {


        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task UpdateService(ViewModel vm, int id)
        {
            T entity = _mapper.Map<T>(vm);
            await _repository.Update(entity, id);
        }

        public async Task<ViewModel> AddService(ViewModel vm)
        {
            T entity = _mapper.Map<T>(vm);
            entity = await _repository.Add(entity);

            vm = _mapper.Map<ViewModel>(entity);
            return vm;
        }

        public async Task DeleteService(ViewModel vm)
        {
            T entity = _mapper.Map<T>(vm);
            await _repository.Delete(entity);
        }

        public async Task<ViewModel> GetByIdService(int id)
        {
            T entity = await _repository.GetById(id);
            ViewModel vm = _mapper.Map<ViewModel>(entity);

            return vm;
        }

        public virtual async Task<ICollection<ViewModel>> GetAllService()
        {
            List<T> entities = await _repository.GetAll();
            List<ViewModel> vms = _mapper.Map<List<ViewModel>>(entities);

            return vms;
        }

    }
}
