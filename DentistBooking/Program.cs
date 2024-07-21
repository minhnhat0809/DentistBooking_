
using Repository.Impl;
using Repository;
using Service.Impl;
using Service;
using AutoMapper;
using DentistBooking.AppStart;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Service.Lib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Index", "/home");
    options.Conventions.AddPageRoute("/GuestPage/BookingAppointment", "/clinic-booking");
    options.Conventions.AddPageRoute("/GuestPage/ClinicInfo", "/clinic-info");
    options.Conventions.AddPageRoute("/GuestPage/ServiceInfo", "/clinic-services");

    options.Conventions.AddPageRoute("/ForgetPassword", "/forget-password");
    options.Conventions.AddPageRoute("/LoginPage", "/login");
    options.Conventions.AddPageRoute("/AdminPage/Users/Index", "/users-management");
    options.Conventions.AddPageRoute("/AdminPage/Services/Index", "/services");
    options.Conventions.AddPageRoute("/AdminPage/Clinics/Index", "/clinics");
    options.Conventions.AddPageRoute("/AdminPage/DentistSlots/Index", "/dentist-slots");
    options.Conventions.AddPageRoute("/AdminPage/MedicalRecords/Index", "/medical-records");
    options.Conventions.AddPageRoute("/AdminPage/Medicines/Index", "/medicine");

    options.Conventions.AddPageRoute("/DentistPage/Customers/Index", "/customers");
    options.Conventions.AddPageRoute("/DentistPage/Customers/MedicalRecords/Index", "/customers/medical-records");
    options.Conventions.AddPageRoute("/DentistPage/Appointments/Index", "/appointments");


    /*options.Conventions.AddPageRoute("/DentistPage/Prescriptions/Index", "/prescriptions");
    options.Conventions.AddPageRoute("/DentistPage/Precriptions/PrecriptionMedicones/Index", "/prescriptions/medicine");*/

    options.Conventions.AddPageRoute("/CustomerPage/ViewServices", "/services/view");
    options.Conventions.AddPageRoute("/CustomerPage/ViewPrescriptions", "/prescriptions/view");
    options.Conventions.AddPageRoute("/CustomerPage/ViewClinic", "/clinic/view");
    options.Conventions.AddPageRoute("/CustomerPage/ViewAppointments", "/appointment/view");
    options.Conventions.AddPageRoute("/CustomerPage/Book", "/book");

});

builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureAutoMapper();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICheckupScheduleService, CheckupScheduleService>();
builder.Services.AddScoped<IDentistSlotService, DentistSlotService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IService, Service.Impl.Service>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IDentistService, DentistService>();
builder.Services.AddScoped<IPrescriptionMedicinesService,  PrescriptionMedicinesService>();
builder.Services.AddScoped<IRoomService,  RoomService>();



//Repositories
builder.Services.AddScoped<IAppointmentRepo, AppointmentRepo>();
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<ICheckupScheduleRepo, CheckupScheduleRepo>();
builder.Services.AddScoped<IDentistSlotRepo, DentistSlotRepo>();
builder.Services.AddScoped<IMedicalRecordRepo, MedicalRecordRepo>();
builder.Services.AddScoped<IPrescriptionrepo, PrescriptionRepo>();
builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IDentistServiceRepo, DentistServiceRepo>();
builder.Services.AddScoped<IMedicineRepo, MedicineRepo>();
builder.Services.AddScoped<IPrescriptionMedicineRepo, PrescriptionMedicineRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IRoomRepo, RoomRepo>();

//Configure Email sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<SignalRHub>("/SignalRHub");

app.UseSession();

app.Run();
//dotnet ef dbcontext scaffold "Server=(local);Initial Catalog=Booking_Dentist_DB;Persist Security Info=False;User ID=sa;Password=12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;" "Microsoft.EntityFrameworkCore.SqlServer"--force
