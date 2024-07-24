using BusinessObject;

namespace Repository
{
    public interface IServiceRepo
    {
        Task<Service> GetServiceByID(int? id);
        Task<List<Service>> GetAllServices();
        Task DeleteService(Service service);
        Task CreateService(Service service);
        Task UpdateService(Service service);

        Task<List<Service>> GetAllServicesAsync();
        Task<IEnumerable<Service>> GetServicesByDentistSlotAsync(int dentistSlotId);

    }
}
