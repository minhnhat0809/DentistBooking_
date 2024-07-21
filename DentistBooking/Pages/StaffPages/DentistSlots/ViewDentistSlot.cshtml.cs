using System.Collections;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace DentistBooking.Pages.StaffPages.DentistSlots;

public class ViewDentistSlot : PageModel
{
    private readonly IDentistSlotService dentistSlotService;
    private readonly IUserService userService;
    private readonly IRoomService _roomService;
    public ViewDentistSlot(IDentistSlotService dentistSlotService, IUserService userService, IRoomService roomService)
    {
        this.dentistSlotService = dentistSlotService;
        this.userService = userService;
        this._roomService = roomService;
    }
    
    [BindProperty(SupportsGet = true)]
    public int? SelectedDentistId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDateForDentist { get; set; }

    public IList<UserDto> Dentists = default!;
    
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeStart { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public TimeOnly DentistSlotTimeEnd { get; set; } = default!;

    public IList<DentistSlotDto> DentistSlots = default!;

    public IList<Room> Rooms { get; set; } = default!;
    
    public int RoomId { get; set; }
    public async Task<IActionResult> OnGet()
    {
        Dentists = await userService.GetAllUserByType("Dentist");
        Rooms =  (await _roomService.GetAllActiveRooms()) .Rooms;

        return Page();
    }

    public async Task<IActionResult> OnPostViewDentistSlot()
    {
        DentistSlots = dentistSlotService.GetAllDentistSlotsByDentistAndDate((int)SelectedDentistId, 
            SelectedDate).Result;

        Dentists = await userService.GetAllUserByType("Dentist");
        return Page();
    }

    public async Task<IActionResult> OnPostCreateDentistSlot()
    {
        var date = SelectedDateForDentist;
        DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
        DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);
        
        DentistSlotResult result = await dentistSlotService.CreateDentistSlot(SelectedDentistId.Value, slotTimeStart, slotTimeEnd, RoomId);
        if (!result.Message.Equals("Success"))
        {
            TempData["ErrorDentistSlot"] = result.Message;
            Dentists = await userService.GetAllUserByType("Dentist");
            return Page();
        }

        TempData["SuccessDentistSlot"] = "Dentist slot create successfully!";
        Dentists = await userService.GetAllUserByType("Dentist");
        return Page();
    }
}