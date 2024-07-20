
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public async Task CreateService(ServiceDto service)
        {
            try
            {
                var model = await _servicerRepo.GetServiceByID(service.ServiceId);
                if (model != null)
                {
                    throw new InvalidOperationException("Service exist yet!");
                }
                model = _mapper.Map<BusinessObject.Service>(service);   
                await _servicerRepo.CreateService(model);
            }
            catch (Exception ex)
            {

                throw new Exception("error at create service", ex);
            }
        }

        public async Task DeleteService(ServiceDto service)
        {
            try
            {
                var model = await _servicerRepo.GetServiceByID(service.ServiceId);
                if (model == null)
                {
                    throw new InvalidOperationException("Services not found!");
                }
                model = _mapper.Map<BusinessObject.Service>(service);
                await _servicerRepo.DeleteService(model);
            }
            catch (Exception ex)
            {

                throw new Exception("error at delete service", ex);
            }
        }

        public async Task<List<ServiceDto>> GetAllServices()
        {
            
            try
            {
                var model =  await _servicerRepo.GetAllServices();
                if (model == null)
                {
                    throw new InvalidOperationException("Services not found!");
                }
                var viewModels = _mapper.Map<List<ServiceDto>>(model);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new Exception("error at delete service", ex);
            }
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            try
            {
                var models = await _servicerRepo.GetAllServicesAsync();
                if (models == null)
                {
                    throw new InvalidOperationException("Services not found!");
                }
                var viewModels = _mapper.Map<ServiceDto[]>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get services", ex);
            }
        }

        public async Task< List<ServiceDto> >GetAllServicesForCustomer(int serviceId)
        {
            List<BusinessObject.Service> services = await _servicerRepo.GetAllServices();

            BusinessObject.Service service = services.FirstOrDefault(s => s.ServiceId == serviceId);
            if (service != null)
            {
                services.Remove(service);
                services.Insert(0, service);
            }
            var viewModels = _mapper.Map<List<ServiceDto>>(services);
            return viewModels;
        }

        public async Task<ServiceDto> GetServiceByID(int id)
        {
            try
            {
                var model = await _servicerRepo.GetServiceByID(id);
                if (model == null)
                {
                    throw new InvalidOperationException("Service not found!");
                }
                var viewModel = _mapper.Map<ServiceDto>(model);
                return viewModel;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get service", ex);
            }
        }

       

        public async Task<IEnumerable<ServiceDto>> GetServicesByDentistSlotAsync(int dentistSlotId)
        {
           
            try
            {
                var model = await _servicerRepo.GetServicesByDentistSlotAsync(dentistSlotId);
                if (model == null)
                {
                    throw new InvalidOperationException("Service not found!");
                }
                var viewModel = _mapper.Map<ServiceDto[]>(model);
                return viewModel;
            }
            catch (Exception ex)
            {

                throw new Exception("error at get service", ex);
            }
        }

        public async Task<ListServiceResult> GetAllActiveServices()
        {
            ListServiceResult listServiceResult = new ListServiceResult();
            try
            {
                var models =  await _servicerRepo.GetAllServicesAsync();
                models = models.Where(m => m.Status == true).ToList();
                if (models == null)
                {
                    listServiceResult.Message = "There are no active services!";
                    return listServiceResult;
                }
                var viewModels = _mapper.Map<List<ServiceDto>>(models);
                listServiceResult.Services = viewModels;
                listServiceResult.Message = "Success";
                return listServiceResult;
            }
            catch (Exception ex)
            {
                listServiceResult.Message = ex.Message;
                return listServiceResult;
            }
        }

        public async Task UpdateService(ServiceDto service)
        {
            try
            {
                var model = await _servicerRepo.GetServiceByID(service.ServiceId);   
                if (model == null)
                {
                    throw new InvalidOperationException("Service not found!");
                }
                model = _mapper.Map<BusinessObject.Service>(service); 
                await _servicerRepo.UpdateService(model); 
            }
            catch (Exception ex)
            {

                throw new Exception("error at get service", ex);
            }
        }
    }
}
