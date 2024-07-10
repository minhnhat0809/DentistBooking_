using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.StaffPages.DentistSlots;

public class ViewDentistSlot : PageModel
{
    private readonly IDentistSlotService dentistSlotService;
    private readonly IUserService userService;
    public ViewDentistSlot(IDentistSlotService dentistSlotService, IUserService userService)
    {
        this.dentistSlotService = dentistSlotService;
        this.userService = userService;
    }
    
    [BindProperty(SupportsGet = true)]
    public int? SelectedDentistId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDate { get; set; }

    public IList<UserDto> Dentists = default!;
    
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeStart { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeEnd { get; set; } = default!;

    public IList<DentistSlotDto> DentistSlots = default!;
    public void OnGet()
    {
        Dentists = userService.GetAllUserByType("Dentist").Result;
    }

    public async Task<IActionResult> OnPostViewDentistSlot()
    {
        DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentistAndDate((int)SelectedDentistId, 
            SelectedDate);

        Dentists = await userService.GetAllUserByType("Dentist");
        return Page();
    }

    public async Task<IActionResult> OnPostCreateDentistSlot()
    {
        var date = SelectedDate;
        DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
        DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);
        
        string result = await dentistSlotService.CreateDentistSlot(SelectedDentistId.Value, slotTimeStart, slotTimeEnd);
        if (!result.Equals("Success"))
        {
            TempData["DentistSlot"] = result;
        }

        TempData["DentistSlot"] = "Dentist slot create successfully!";
        Dentists = await userService.GetAllUserByType("Dentist");
        return Page();
    }
}