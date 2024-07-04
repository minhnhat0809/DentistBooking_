using BusinessObject;

namespace Service
{
    public interface IService
    {
       
        public BusinessObject.Service GetServiceByID(int id);
        public List<BusinessObject.Service> GetAllServices();
        public void DeleteService(BusinessObject.Service service);
        public void CreateService(BusinessObject.Service service);
        public void UpdateService(BusinessObject.Service service);
        List<BusinessObject.Service> GetAllServicesForCustomer(int serviceId);
    }
}
