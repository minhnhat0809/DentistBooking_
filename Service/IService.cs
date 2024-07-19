using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IService
    {
       
        Task<ServiceDto> GetServiceByID(int id);
        Task<List<ServiceDto>> GetAllServices();
        Task DeleteService(ServiceDto service);
        Task CreateService(ServiceDto service);
        Task UpdateService(ServiceDto service);
        Task<List<ServiceDto>> GetAllServicesForCustomer(int serviceId);
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<IEnumerable<ServiceDto>> GetServicesByDentistSlotAsync(int dentistSlotId);
    }
}
