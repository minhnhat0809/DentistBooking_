using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IService
    {
       
        public Task<ServiceDto> GetServiceByID(int id);
        public Task<List<ServiceDto>> GetAllServices();
        public void DeleteService(ServiceDto service);
        public void CreateService(ServiceDto service);
        public void UpdateService(ServiceDto service);
        Task<List<ServiceDto>> GetAllServicesForCustomer(int serviceId);
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<IEnumerable<ServiceDto>> GetServicesByDentistSlotAsync(int dentistSlotId);
    }
}
