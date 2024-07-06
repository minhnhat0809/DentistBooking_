using BusinessObject;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository.Impl
{
    public class ServiceRepo : IServiceRepo
    {
        public void CreateService(Service service)
        => ServiceDAO.Instance.createService(service);

        public void DeleteService(Service service)
        => ServiceDAO.Instance.deleteService(service);


        public async Task<List<Service>> GetAllServices()
        => await ServiceDAO.Instance.getAllServices();

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        => await ServiceDAO.Instance.getAllServices();

        public async Task<Service> GetServiceByID(int? id)
        => await ServiceDAO.Instance.getServiceByID(id);

        public async Task<IEnumerable<Service>> GetServicesByDentistSlotAsync(int dentistSlotId)
        => await ServiceDAO.Instance.GetServicesByDentistSlotAsync(dentistSlotId);

        public void UpdateService(Service service)
        =>ServiceDAO.Instance.updateService(service);
    }
}
