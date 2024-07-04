
using AutoMapper;
using BusinessObject.DTO;
using Repository;

namespace Service.Impl
{
    public class Service : IService
    {
        private readonly IServiceRepo _servicerRepo;
        private readonly IMapper _mapper;
        public Service(IServiceRepo repo, IMapper mapper)
        {
            _servicerRepo = repo;
            _mapper = mapper;
        }
        public void CreateService(BusinessObject.Service service)
        => _servicerRepo.CreateService(service);

        public void DeleteService(BusinessObject.Service service)
        =>_servicerRepo.DeleteService(service);

        public async Task<List<ServiceDto>> GetAllServices()
        {
            List<BusinessObject.Service> models = await _servicerRepo.GetAllServices();
            List<ServiceDto> viewModels = _mapper.Map<List<ServiceDto>>(models);
            return viewModels;
        }

        public List<BusinessObject.Service> GetAllServicesForCustomer(int serviceId)
        {
            List<BusinessObject.Service> services = _servicerRepo.GetAllServices().Result;

            BusinessObject.Service service = services.FirstOrDefault(s => s.ServiceId == serviceId);
            if (service != null)
            {
                services.Remove(service);
                services.Insert(0, service);
            }
            return services;
        }

        public async Task<ServiceDto> GetDtoById(int id)
        {
            BusinessObject.Service model = await _servicerRepo.GetServiceByID(id);
            return _mapper.Map<ServiceDto>(model);
        }

        

        public BusinessObject.Service GetServiceByID(int id)
        {
            return _servicerRepo.GetServiceByID(id).Result;
        }

        public void UpdateService(BusinessObject.Service service)
        => _servicerRepo.UpdateService(service);
    }
}
