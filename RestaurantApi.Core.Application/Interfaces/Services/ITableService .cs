using RestaurantApi.Core.Application.ViewModels;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Application.ViewModels.Table;
using RestaurantApi.Core.Domain.Entities;

namespace RestaurantApi.Core.Application.Interfaces.Services
{
    public interface ITableService : IGenericService<TableViewModel, Table>
    {
        public Task<ICollection<ListTableViewModel>> GetAllServiceList();
        public Task UpdateService(UpdateTableViewModel vm, int id);
        public Task<List<OrderViewModel>> GetTableOrden(int id);

        public Task ChangeStatusService(ChangeStatusTableViewModel vm, int id);



    }
}
