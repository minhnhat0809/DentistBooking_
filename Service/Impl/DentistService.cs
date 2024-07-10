using AutoMapper;
using BusinessObject.DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class DentistService : IDentistService
    {
        private readonly IDentistServiceRepo dentistServiceRepo;
        private readonly IServiceRepo serviceRepo;
        private readonly IMapper mapper;
        public DentistService(IDentistServiceRepo dentistServiceRepo, IServiceRepo serviceRepo, IMapper mapper)
        {
            this.dentistServiceRepo = dentistServiceRepo;
            this.serviceRepo = serviceRepo;
            this.mapper = mapper;
        }


        public async Task<List<ServiceDto>> GetAllServiceByDentist(int dentistId, int serviceId)
        {
            List<BusinessObject.Service> services = await dentistServiceRepo.GetAllServiceByDentist(dentistId);
            BusinessObject.Service service = await serviceRepo.GetServiceByID(serviceId);

            foreach (var s in services)
            {
                if (s.ServiceId == serviceId)
                {
                    services.Remove(s);
                    services.Insert(0, service);
                    break;
                }
            }
            var viewModels = mapper.Map<List<ServiceDto>>(services);
            return viewModels;
        }


    }
}
