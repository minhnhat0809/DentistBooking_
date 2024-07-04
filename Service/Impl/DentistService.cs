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
        private readonly IService service;
        public DentistService(IDentistServiceRepo dentistServiceRepo, IService service)
        {
            this.dentistServiceRepo = dentistServiceRepo;
            this.service = service;
        }


        public List<BusinessObject.Service> GetAllServiceByDentist(int dentistId, int serviceId)
        {
            List<BusinessObject.Service> services = dentistServiceRepo.GetAllServiceByDentist(dentistId);
            BusinessObject.Service sErvice = service.GetServiceByID(serviceId);

            foreach (var s in services)
            {
                if (s.ServiceId == serviceId)
                {
                    services.Remove(s);
                    services.Insert(0, sErvice);
                    break;
                }
            }
            return services;
        }


    }
}
