using AutoMapper;
using RestaurantApi.Core.Application.Interfaces.Repositories;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Application.ViewModels.Table;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Services
{
    public class TableService : GenericService<TableViewModel, Table> , ITableService
    {

        private readonly IMapper _mapper;
        private readonly ITableRepository _repository;
        private readonly IOrderRepository _orderRepository;

        public TableService(IGenericRepository<Table> repository, IMapper mapper, 
            ITableRepository tableRepository, IOrderRepository orderRepository) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = tableRepository;
            _orderRepository = orderRepository;
        }


        public async Task<ICollection<ListTableViewModel>> GetAllServiceList()
        {
            List<Table> entities = await _repository.GetAll();
            List<ListTableViewModel> vms = _mapper.Map<List<ListTableViewModel>>(entities);

            return vms;
        }

        public async Task UpdateService(UpdateTableViewModel vm, int id)
        {
            Table entity = _mapper.Map<Table>(vm);
            await _repository.Update(entity, id);
        }

        public async Task ChangeStatusService(ChangeStatusTableViewModel vm, int id)
        {
            Table entity = _mapper.Map<Table>(vm);
            await _repository.Update(entity, id);
        }

        public async Task<List<OrderViewModel>> GetTableOrden(int id)
        {
            List<Order> orders = await _orderRepository.GetAllWithInclude();
            List<Order> tableOrders = orders.Where(x => x.TableId == id ).ToList();

            var result = _mapper.Map<List<OrderViewModel>>(tableOrders);

            return result;
        }


    }
}
