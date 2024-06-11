using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public partial class BookingDentistDbContext : DbContext
{
    public BookingDentistDbContext()
    {
    }

    public BookingDentistDbContext(DbContextOptions<BookingDentistDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<CheckupSchedule> CheckupSchedules { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<CustomerDentistSlot> CustomerDentistSlots { get; set; }

    public virtual DbSet<DentistSlot> DentistSlots { get; set; }

    public virtual DbSet<MedicalRecordDAO> MedicalRecords { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceAppointment> ServiceAppointments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__A50828FCFCC5C6E4");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("appointment_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DentistSlotId).HasColumnName("dentist_slot_id");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.MedicalRecordId).HasColumnName("medical_record_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.DentistSlot).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DentistSlotId)
                .HasConstraintName("FK__appointme__denti__4CA06362");

            entity.HasOne(d => d.MedicalRecord).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.MedicalRecordId)
                .HasConstraintName("FK__appointme__medic__4D94879B");
        });

        modelBuilder.Entity<CheckupSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__checkup___C46A8A6F6431DA91");

            entity.ToTable("checkup_schedule");

            entity.Property(e => e.ScheduleId)
                .ValueGeneratedNever()
                .HasColumnName("schedule_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DentistId).HasColumnName("dentist_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("time_start");

            entity.HasOne(d => d.Customer).WithMany(p => p.CheckupScheduleCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__checkup_s__custo__5AEE82B9");

            entity.HasOne(d => d.Dentist).WithMany(p => p.CheckupScheduleDentists)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__checkup_s__denti__5BE2A6F2");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.ClinicId).HasName("PK__clinic__A0C8D19BA982DF03");

            entity.ToTable("clinic");

            entity.Property(e => e.ClinicId)
                .ValueGeneratedNever()
                .HasColumnName("clinic_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.ClinicName)
                .HasMaxLength(255)
                .HasColumnName("clinic_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<CustomerDentistSlot>(entity =>
        {
            entity.HasKey(e => e.CustomerDentistSlotId).HasName("PK__customer__61C8E4E0511ADAC8");

            entity.ToTable("customer_dentist_slot");

            entity.Property(e => e.CustomerDentistSlotId)
                .ValueGeneratedNever()
                .HasColumnName("customer_dentist_slot_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DentistSlotId).HasColumnName("dentist_slot_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerDentistSlots)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__customer___custo__5CD6CB2B");

            entity.HasOne(d => d.DentistSlot).WithMany(p => p.CustomerDentistSlots)
                .HasForeignKey(d => d.DentistSlotId)
                .HasConstraintName("FK__customer___denti__5DCAEF64");
        });

        modelBuilder.Entity<DentistSlot>(entity =>
        {
            entity.HasKey(e => e.DentistSlotId).HasName("PK__dentist___F7C6C8C3141B8B51");

            entity.ToTable("dentist_slot");

            entity.Property(e => e.DentistSlotId)
                .ValueGeneratedNever()
                .HasColumnName("dentist_slot_id");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.DentistId).HasColumnName("dentist_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Dentist).WithMany(p => p.DentistSlots)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__dentist_s__denti__5EBF139D");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.MediaRecordId).HasName("PK__medical___E8DC072EC8E03993");

            entity.ToTable("medical_record");

            entity.Property(e => e.MediaRecordId)
                .ValueGeneratedNever()
                .HasColumnName("media_record_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__medical_r__custo__5FB337D6");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__medicine__E7148EBB3D82D5ED");

            entity.ToTable("medicine");

            entity.Property(e => e.MedicineId)
                .ValueGeneratedNever()
                .HasColumnName("medicine_id");
            entity.Property(e => e.MedicineName)
                .HasMaxLength(255)
                .HasColumnName("medicine_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__prescrip__3EE444F8C1654945");

            entity.ToTable("prescription");

            entity.Property(e => e.PrescriptionId)
                .ValueGeneratedNever()
                .HasColumnName("prescription_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__prescript__appoi__5441852A");
        });

        modelBuilder.Entity<PrescriptionMedicine>(entity =>
        {
            entity.HasKey(e => e.PrescriptionMedicineId).HasName("PK__prescrip__6C802F922E8348D8");

            entity.ToTable("prescription_medicine");

            entity.Property(e => e.PrescriptionMedicineId)
                .ValueGeneratedNever()
                .HasColumnName("prescription_medicine_id");
            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrescriptionMedicines)
                .HasForeignKey(d => d.MedicineId)
                .HasConstraintName("FK__prescript__medic__59FA5E80");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionMedicines)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__prescript__presc__59063A47");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__role__760965CCD33EE017");

            entity.ToTable("role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__service__3E0DB8AF6EB1286B");

            entity.ToTable("service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("service_id");
            entity.Property(e => e.ClinicOwnerId).HasColumnName("clinic_owner_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("service_name");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.ClinicOwner).WithMany(p => p.Services)
                .HasForeignKey(d => d.ClinicOwnerId)
                .HasConstraintName("FK__service__clinic___60A75C0F");
        });

        modelBuilder.Entity<ServiceAppointment>(entity =>
        {
            entity.HasKey(e => e.ServiceAppointmentId).HasName("PK__service___5A8BD615CC61AC18");

            entity.ToTable("service_appointment");

            entity.Property(e => e.ServiceAppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("service_appointment_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.ServiceAppointments)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__service_a__appoi__5165187F");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceAppointments)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__service_a__servi__5070F446");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user__B9BE370F42EAC76C");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Users)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__user__clinic_id__3D5E1FD2");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__user__role_id__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }
}
