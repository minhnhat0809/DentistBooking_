using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IService
    {
       
        public BusinessObject.Service GetServiceByID(int id);
        public Task<ServiceDto> GetDtoById(int id);
        public Task<List<ServiceDto>> GetAllServices();
        public void DeleteService(BusinessObject.Service service);
        public void CreateService(BusinessObject.Service service);
        public void UpdateService(BusinessObject.Service service);
        List<BusinessObject.Service> GetAllServicesForCustomer(int serviceId);
    }
}
