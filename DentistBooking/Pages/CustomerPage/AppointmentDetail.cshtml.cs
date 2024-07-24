using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<SignalRHub> hubContext;
        public AppointmentDetailModel(IAppointmentService appointmentService, IDentistService dentistService, IService service, IUserService _userService, IHubContext<SignalRHub> hubContext)
        {
            this.appointmentService = appointmentService;   
            this.dentistService = dentistService;
            this.service = service;
            this._userService = _userService;
            this.hubContext = hubContext;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public IList<ServiceDto> Services { get; set; } = default!;

        public IList<UserDto> Dentists { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string CustomerName { get; set; }
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
            hubContext.Clients.All.SendAsync("ReloadAppointments");
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
        
        public async Task<IActionResult> OnPostDelete(int appointmentId)
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                TempData["ErrorDeleteAppointment"] = "Customer Name are required.";
                return RedirectToPage(new {id = appointmentId});
            }
            
            AppointmentResult result = appointmentService.DeleteAppointmentForStaff(appointmentId, CustomerName, "");
            
            await hubContext.Clients.All.SendAsync("ReloadAppointments");
            
            if (!result.Message.Equals("Success"))
            {
                TempData["ErrorDeleteCustomerAppointment"] = result.Message;
                Appointment = await appointmentService.GetAppointmentByID(appointmentId);
                Dentists = await dentistService.GetDentistsForAppointmentCustomer(appointmentId);
                Services = await service.ServicesForAppointmentCustomer(appointmentId);
                return RedirectToPage(new {id = appointmentId});
            }

            TempData["SuccessDeleteCustomerAppointment"] = "Delete successfully!";
            return RedirectToPage("/CustomerPage/ViewAppointments");
        }
    }
}
