using BusinessObject;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository.Impl
{
    public class ServiceRepo : IServiceRepo
    {
        public async Task CreateService(Service service)
        => await ServiceDAO.Instance.createService(service);

        public async Task DeleteService(Service service)
        => await ServiceDAO.Instance.deleteService(service);


        public async Task<List<Service>> GetAllServices()
        => await ServiceDAO.Instance.getAllServices();

        public async Task<List<Service>> GetAllServicesAsync()
        => await ServiceDAO.Instance.getAllServices();

        public async Task<Service> GetServiceByID(int? id)
        => await ServiceDAO.Instance.getServiceByID(id);

        public async Task<IEnumerable<Service>> GetServicesByDentistSlotAsync(int dentistSlotId)
        => await ServiceDAO.Instance.GetServicesByDentistSlotAsync(dentistSlotId);

        public async Task UpdateService(Service service)
        => await ServiceDAO.Instance.updateService(service);
    }
}
