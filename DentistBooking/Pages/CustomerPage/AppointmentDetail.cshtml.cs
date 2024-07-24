using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Service;

namespace DentistBooking.Pages.CustomerPage
{
    public class AppointmentDetailModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDentistService dentistService;
        private readonly IUserService _userService;
        private readonly IService service;
        public AppointmentDetailModel(IAppointmentService appointmentService, IDentistService dentistService, IService service, IUserService _userService)
        {
            this.appointmentService = appointmentService;   
            this.dentistService = dentistService;
            this.service = service;
            this._userService = _userService;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;

        public IList<UserDto> Dentists { get; set; } = default!;
        public async Task<IActionResult>  OnGet(int id)
        {
            Appointment = await appointmentService.GetAppointmentByID(id);
            Dentists = await dentistService.GetDentistsForAppointmentCustomer(id);
            Services = await service.ServicesForAppointmentCustomer(id);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate()
        {

            Dictionary<string, string> result = await appointmentService.UpdateAppointment((int)Appointment.ServiceId, 
                Appointment.AppointmentId, Appointment.TimeStart, (int)Appointment.CustomerId, Appointment.CreateBy.Value);
            if (!result.ContainsKey("Success"))
            {
                foreach(var item in result)
                {
                    TempData["AppointmentDetail"] = item.Value;
                }
                Appointment = await appointmentService.GetAppointmentByID(Appointment.AppointmentId);
                Services = await service.ServicesForAppointmentCustomer(Appointment.AppointmentId);
                Dentists = await dentistService.GetDentistsForAppointmentCustomer(Appointment.AppointmentId);
                return Page();
            }

            TempData["AppointmentDetail"] = "Appointment updated successfully!";
            Appointment = await appointmentService.GetAppointmentByID(Appointment.AppointmentId);
            Services = await service.ServicesForAppointmentCustomer(Appointment.AppointmentId);
            Dentists = await dentistService.GetDentistsForAppointmentCustomer(Appointment.AppointmentId);
            return Page();
        }
        
        public JsonResult OnGetDentistByServiceAsync(int serviceId)
        {

            var dentists =  _userService.GetAllDentistsByService(serviceId).Result;
            if (dentists.IsNullOrEmpty())
            {
                return new JsonResult(new { success = false, message = "No dentists found for this service." });
            }
            var dentistSelectList = dentists.Select(s => new SelectListItem
            {
                Value = s.UserId.ToString(),
                Text = s.Name
                    
            }).ToList();

            return new JsonResult(new { success = true, dentistSelectList });
        }
    }
}
