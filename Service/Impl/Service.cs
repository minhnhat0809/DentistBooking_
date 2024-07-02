
using Repository;

namespace Service.Impl
{
    public class Service : IService
    {
        private readonly IServiceRepo _servicerRepo;
        public Service(IServiceRepo repo)
        {
            _servicerRepo = repo;
        }
        public void CreateService(BusinessObject.Service service)
        => _servicerRepo.CreateService(service);

        public void DeleteService(BusinessObject.Service service)
        =>_servicerRepo.DeleteService(service);

        public List<BusinessObject.Service> GetAllServices()
        => _servicerRepo.GetAllServices();

        public List<BusinessObject.Service> GetAllServicesForCustomer(int serviceId)
        {
            List<BusinessObject.Service> services = _servicerRepo.GetAllServices();

            BusinessObject.Service service = services.FirstOrDefault(s => s.ServiceId == serviceId);
            if (service != null)
            {
                services.Remove(service);
                services.Insert(0, service);
            }
            return services;
        }

        public BusinessObject.Service GetServiceByID(int id)
        => _servicerRepo.GetServiceByID(id);

        public void UpdateService(BusinessObject.Service service)
        => _servicerRepo.UpdateService(service);
    }
}
