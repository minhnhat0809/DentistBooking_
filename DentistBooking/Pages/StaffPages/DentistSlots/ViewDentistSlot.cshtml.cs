using System.Collections;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Service;

namespace DentistBooking.Pages.StaffPages.DentistSlots;

public class ViewDentistSlot : PageModel
{
    private readonly IDentistSlotService dentistSlotService;
    private readonly IUserService userService;
    private readonly IRoomService _roomService;
    private readonly IHubContext<SignalRHub> hubContext;
    public ViewDentistSlot(IDentistSlotService dentistSlotService, IUserService userService, IRoomService roomService, IHubContext<SignalRHub> hubContext)
    {
        this.dentistSlotService = dentistSlotService;
        this.userService = userService;
        this._roomService = roomService;
        this.hubContext = hubContext;
    }
    
    [BindProperty(SupportsGet = true)]
    public int SelectedDentistId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly SelectedDateForDentist { get; set; }

    public IList<UserDto> Dentists = default!;
    
    [BindProperty]
    public string SlotTimeRange { get; set; }

    public IList<DentistSlotDto> DentistSlots = new List<DentistSlotDto>();

    public IList<Room> Rooms { get; set; } = default!;
    
    [BindProperty (SupportsGet = true)]
    public int RoomId { get; set; }
    public async Task<IActionResult> OnGet()
    {
        string role = HttpContext.Session.GetString("Role");
        if (!role.IsNullOrEmpty())
        {
            if (!role.Equals("Staff"))
            {
                return RedirectToPage("/Index");
            }
        }
        else
        {
            return RedirectToPage("/Index");
        }
        Dentists =  userService.GetAllUserByType("Dentist").Result;
        Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
        DentistSlots = dentistSlotService.GetAllDentistSlotsByDentistAndDate((userService.GetAllUserByType("Dentist").Result)[0].UserId, 
            DateOnly.FromDateTime(DateTime.Now)).Result;
        
        return Page();
    }

    public async Task<IActionResult> OnPostViewDentistSlot()
    {
        string role = HttpContext.Session.GetString("Role");
        if (!role.IsNullOrEmpty())
        {
            if (!role.Equals("Staff"))
            {
                return RedirectToPage("/Index");
            }
        }
        else
        {
            return RedirectToPage("/Index");
        }
        DentistSlots = dentistSlotService.GetAllDentistSlotsByDentistAndDate(SelectedDentistId, 
            SelectedDate).Result;

        Dentists =  userService.GetAllUserByType("Dentist").Result;
        Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
        return Page();
    }

    public async Task<IActionResult> OnPostCreateDentistSlot()
    {
        string role = HttpContext.Session.GetString("Role");
        if (!role.IsNullOrEmpty())
        {
            if (!role.Equals("Staff"))
            {
                return RedirectToPage("/Index");
            }
        }
        else
        {
            return RedirectToPage("/Index");
        }
        var date = SelectedDateForDentist;
        var timeRange = SlotTimeRange.Split('-');
        TimeOnly DentistSlotTimeStart = TimeOnly.Parse(timeRange[0]);
        TimeOnly DentistSlotTimeEnd = TimeOnly.Parse(timeRange[1]);
        
        DateTime slotTimeStart = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeStart.Hour, DentistSlotTimeStart.Minute, DentistSlotTimeStart.Second);
            
        DateTime slotTimeEnd = new DateTime(date.Year, date.Month, date.Day,
            DentistSlotTimeEnd.Hour, DentistSlotTimeEnd.Minute, DentistSlotTimeEnd.Second);
        
        DentistSlotResult result = await dentistSlotService.CreateDentistSlot(SelectedDentistId, slotTimeStart, slotTimeEnd, RoomId);
        if (!result.Message.Equals("Success"))
        {
            TempData["ErrorDentistSlot"] = result.Message;
            Dentists =  userService.GetAllUserByType("Dentist").Result;
            Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
            return Page();
        }

        TempData["SuccessDentistSlot"] = "Dentist slot create successfully!";
        await hubContext.Clients.All.SendAsync("ReloadDentistSlots");
        Dentists =  userService.GetAllUserByType("Dentist").Result;
        Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
        return Page();
    }
    
    public async Task<IActionResult> OnPostReactivateAsync(int slotId)
    {
        string role = HttpContext.Session.GetString("Role");
        if (!role.IsNullOrEmpty())
        {
            if (!role.Equals("Staff"))
            {
                return RedirectToPage("/Index");
            }
        }
        else
        {
            return RedirectToPage("/Index");
        }
        DentistSlotResult dentistSlotResult = await dentistSlotService.UpdateStatusDentistSlot(true, slotId);
        Dentists =  userService.GetAllUserByType("Dentist").Result;
        Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
        DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentistAndDate((userService.GetAllUserByType("Dentist").Result)[0].UserId, 
            DateOnly.FromDateTime(DateTime.Now));
        if (!dentistSlotResult.Message.Contains("Success"))
        {
            TempData["ErrorDentistSlot"] = dentistSlotResult.Message;
            return Page();
        }
        TempData["SuccessDentistSlot"] = "Slot reactivated successfully.";
        await hubContext.Clients.All.SendAsync("ReloadDentistSlots");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int slotId)
    {
        string role = HttpContext.Session.GetString("Role");
        if (!role.IsNullOrEmpty())
        {
            if (!role.Equals("Staff"))
            {
                return RedirectToPage("/Index");
            }
        }
        else
        {
            return RedirectToPage("/Index");
        }
        DentistSlotResult dentistSlotResult = await dentistSlotService.UpdateStatusDentistSlot(false, slotId);
        Dentists =  userService.GetAllUserByType("Dentist").Result;
        Rooms =  _roomService.GetAllActiveRooms().Result.Rooms;
        DentistSlots = await dentistSlotService.GetAllDentistSlotsByDentistAndDate((userService.GetAllUserByType("Dentist").Result)[0].UserId, 
            DateOnly.FromDateTime(DateTime.Now));
        if (!dentistSlotResult.Message.Contains("Success"))
        {
            TempData["ErrorDentistSlot"] = dentistSlotResult.Message;
            return Page();
        }
        TempData["SuccessDentistSlot"] = "Slot deleted successfully.";
        await hubContext.Clients.All.SendAsync("ReloadDentistSlots");
        return RedirectToPage();
    }
}