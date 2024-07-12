namespace RestaurantApi.Core.Application.Interfaces.Services
{
    public interface IGenericService<ViewModel, T> where ViewModel : class where T : class
    {
        Task<ICollection<ViewModel>> GetAllService();
        Task<ViewModel> GetByIdService(int id);
        Task<ViewModel> AddService(ViewModel vm);
        Task UpdateService(ViewModel vm, int id);
        Task DeleteService(ViewModel vm);
    }
}
