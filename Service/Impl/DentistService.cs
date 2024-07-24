using AutoMapper;
using BusinessObject.DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service.Impl
{
    public class DentistService : IDentistService
    {
        private readonly IDentistServiceRepo dentistServiceRepo;
        private readonly IServiceRepo serviceRepo;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserRepo _userRepo;
        private readonly IMapper mapper;
        public DentistService(IDentistServiceRepo dentistServiceRepo, IServiceRepo serviceRepo, IAppointmentService _appointmentService, IUserRepo _userRepo, IMapper mapper)
        {
            this.dentistServiceRepo = dentistServiceRepo;
            this.serviceRepo = serviceRepo;
            this._appointmentService = _appointmentService;
            this._userRepo = _userRepo;
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

        public async Task<List<UserDto>> GetDentistsForAppointmentCustomer(int appointmentId)
        {
            AppointmentDto appointment = await _appointmentService.GetAppointmentByID(appointmentId);
            
            var models = await _userRepo.GetAllDentistsByService(appointment.ServiceId.Value);

            if (appointment.CreateBy.HasValue)
            {
                foreach (var m in models)
                {
                    if (m.UserId == appointment.CreateBy.Value)
                    {
                        var c = m;
                        models.Remove(m);
                        models.Insert(0, c);
                        break;
                    }
                }
            }

            return mapper.Map<List<UserDto>>(models);
        }
    }
}
