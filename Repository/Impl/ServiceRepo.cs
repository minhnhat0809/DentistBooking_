using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class ServiceRepo : IServiceRepo
    {
        public void CreateService(Service service)
        => ServiceDAO.Instance.createService(service);

        public void DeleteService(Service service)
        => ServiceDAO.Instance.deleteService(service);


        public List<Service> GetAllServices()
        => ServiceDAO.Instance.getAllServices();

        public Service GetServiceByID(int id)
        => ServiceDAO.Instance.getServiceByID(id);

        public void UpdateService(Service service)
        =>ServiceDAO.Instance.updateService(service);
    }
}
