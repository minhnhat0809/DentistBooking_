using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.StaffPages;

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
    public DateTime SelectedDate { get; set; }

    public IList<User> Dentists = default!;
    
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeStart { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeEnd { get; set; } = default!;

    public IList<DentistSlot> DentistSlots = default!;

    [BindProperty(SupportsGet = true)]
    public int DentistSlotId { get; set; } = default!;
    public void OnGet()
    {
        Dentists = userService.GetAllUserByType("Dentist");
    }

    public IActionResult OnPostViewDentistSlot()
    {
        DentistSlots = dentistSlotService.GetAllDentistSlotsByDentistAndDate((int)SelectedDentistId, 
            DateOnly.FromDateTime(SelectedDate)).Result;

        Dentists = userService.GetAllUserByType("Dentist");
        return Page();
    }

    public IActionResult OnPostCreateDentistSlot()
    {
        var date = SelectedDate;
        DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
        DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);
        
        string result = dentistSlotService.CreateDentistSlot(SelectedDentistId.Value, slotTimeStart, slotTimeEnd);
        if (!result.Equals("Success"))
        {
            TempData["DentistSlot"] = result;
        }

        TempData["DentistSlot"] = "Dentist slot create successfully!";
        Dentists = userService.GetAllUserByType("Dentist");
        return Page();
    }

    public IActionResult OnPostDeleteDentistSlot(int DentistSlotId)
    {
        string result = dentistSlotService.DeleteDentistSlot(DentistSlotId);
        if (result.Equals("Success"))
        {
            TempData["DentistSlot"] = result;
        }

        TempData["DentistSlot"] = "Delete successfully!";
        Dentists = userService.GetAllUserByType("Dentist");
        return Page();
    }
}