
using Repository.Impl;
using Repository;
using Service.Impl;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/ForgetPassword", "/forget-password");
    options.Conventions.AddPageRoute("/Index", "/login");
}); ;


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



//Reposiroties
builder.Services.AddScoped<IAppointmentRepo, AppointmentRepo>();
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<ICheckupScheduleRepo, CheckupScheduleRepo>();
builder.Services.AddScoped<IDentistSlotRepo, DentistSlotRepo>();
builder.Services.AddScoped<IMedicalRecordRepo, MedicalRecordRepo>();
builder.Services.AddScoped<IPrescriptionrepo, PrescriptionRepo>();
builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IDentistServiceRepo, DentistServiceRepo>();
builder.Services.AddScoped<IServiceAppointmentRepo, ServiceAppointmentRepo>();

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

app.Run();
