using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Identity;
using Service.Impl;

namespace DentistBooking.AppStart
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc =>
            {
                // add mapper
                mc.CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();
                mc.CreateMap<CheckupSchedule, CheckupScheduleDto>()
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist.Name))
                    .ForMember(dest => dest.Customer, opt => opt.Ignore())
                    .ForMember(dest => dest.Dentist, opt => opt.Ignore());
                mc.CreateMap<BusinessObject.Service, ServiceDto>().ReverseMap();
                mc.CreateMap<Appointment, AppointmentDto>().ReverseMap();
                mc.CreateMap<CheckupSchedule, CheckupScheduleDto>().ReverseMap();
                mc.CreateMap<Clinic, ClinicDto>().ReverseMap();
                mc.CreateMap<BusinessObject.DentistService, DentistServiceDto>().ReverseMap();
                mc.CreateMap<DentistSlot, DentistSlotDto>().ReverseMap();
                mc.CreateMap<Medicine, MedicineDto>().ReverseMap();
                mc.CreateMap<Prescription, PrescriptionDto>().ReverseMap();
                mc.CreateMap<PrescriptionMedicine, PrescriptionMedicineDto>().ReverseMap();
                mc.CreateMap<Role, RoleDto>().ReverseMap();
                mc.CreateMap<User, UserDto >().ReverseMap();

            });
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
