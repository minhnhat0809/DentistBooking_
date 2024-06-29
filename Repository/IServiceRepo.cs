using BusinessObject;

namespace Repository
{
    public interface IServiceRepo
    {
        public Service GetServiceByID(int id);
        public List<Service> GetAllServices();
        public void DeleteService(Service service);
        public void CreateService(Service service);
        public void UpdateService(Service service);

    }
}
