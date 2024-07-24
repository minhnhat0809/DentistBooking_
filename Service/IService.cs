using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;

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
        Task<List<ServiceDto>> GetAllServicesByDentistId(int dentistId);
        Task<IEnumerable<ServiceDto>> GetServicesByDentistSlotAsync(int dentistSlotId);

        Task<ListServiceResult> GetAllActiveServices();
    }
}
