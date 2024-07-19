using Microsoft.AspNetCore.SignalR;

public class SignalRHub : Hub
{
    public async Task ReloadAppointments()
    {
        await Clients.All.SendAsync("ReloadAppointments");
    }
    public async Task ReloadCheckupSchedules()
    {
        await Clients.All.SendAsync("ReloadCheckupSchedules");
    }
    public async Task ReloadMedicalRecords()
    {
        await Clients.All.SendAsync("ReloadMedicalRecords");
    }
    public async Task ReloadMedicines()
    {
        await Clients.All.SendAsync("ReloadMedicines");
    }
    public async Task ReloadPrescriptions()
    {
        await Clients.All.SendAsync("ReloadPrescriptions");
    }
    public async Task ReloadPrescriptionMedicines()
    {
        await Clients.All.SendAsync("ReloadPrescriptionMedicines");
    }
    public async Task ReloadServices()
    {
        await Clients.All.SendAsync("ReloadServices");
    }
    public async Task ReloadUsers()
    {
        await Clients.All.SendAsync("ReloadUsers");
    }
    public async Task ReloadClinics()
    {
        await Clients.All.SendAsync("ReloadClinics");
    }
}

