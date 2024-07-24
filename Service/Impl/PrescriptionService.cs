using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Repository.Impl;
using Service.Exeption;

namespace Service.Impl
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionrepo _preScription;
        private readonly IPrescriptionMedicineRepo _prescriptionMedicineRepo;
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IMedicineRepo _medicineRepo;
        private readonly IMapper _mapper;

        public PrescriptionService(IPrescriptionrepo prescription, IMapper mapper, IAppointmentRepo appointmentRepo, IMedicineRepo medicineRepo)
        {
            _preScription = prescription ?? throw new ArgumentNullException(nameof(prescription));
            _mapper = mapper;
            _appointmentRepo = appointmentRepo ?? throw new ArgumentNullException();
            _medicineRepo = medicineRepo;
        }

        

        public async Task CreatePrescription(PrescriptionDto prescription)
        {
            if (prescription == null)
            {
                throw new ArgumentNullException(nameof(prescription), "Prescription cannot be null.");
            }

            try
            {
                var model = await _preScription.GetById(prescription.PrescriptionId);
                if (model != null)
                {
                    throw new InvalidOperationException($"Medicine with ID {prescription.PrescriptionId} already exists.");
                }
                model = _mapper.Map<Prescription>(prescription);
                await _preScription.CreatePrescription(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task DeletePrescription(int prescriptionId)
        {
            // when delete set status = inactive and roll back all medicines to storage !
            try
            {
                var prescription = await _preScription.GetById(prescriptionId);
                if (prescription == null)
                {
                    throw new ArgumentException("Prescription not found.");
                }

                // Revert all associated medicines back to stock
                var prescriptionMedicines = await _prescriptionMedicineRepo.GetAllPrescriptionMedicinesByPrescriptionId(prescriptionId);
                foreach (var pm in prescriptionMedicines)
                {
                    var medicine = await _medicineRepo.GetById(pm.MedicineId);
                    if (medicine != null)
                    {
                        medicine.Quantity += pm.Quantity;
                        await _medicineRepo.UpdateMedicine(medicine);
                    }
                }
                prescription.PrescriptionMedicines.Clear();
                // Delete the prescription
                await _preScription.DeletePrescription(prescription.PrescriptionId);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the prescription.", ex);
            }
        }


        public async Task<PrescriptionDto> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid prescription ID.", nameof(id));
            }

            try
            {
                var model =  await _preScription.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Prescription with ID {id} not found.");
                }
                var viewModel = _mapper.Map<PrescriptionDto>(model);    
                return viewModel;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the Prescription.", ex);
            }
        }

        public async Task<PrescriptionDto> GetByIdWithMedicinesAsync(int id)
        {
            try
            {
                var model = await _preScription.GetByIdWithMedicinesAsync(id);
                return _mapper.Map<PrescriptionDto>(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving Prescription.", ex);
            }
        }

        public async Task<List<PrescriptionDto>> GetAllPrescriptionByCustomer(int customerId)
        {
            if (customerId == 0)
            {
                throw new Exception("Customer ID cannot be zero.");
            }

            try
            {
                var models = await _preScription.GetPrescriptions();

                if (models == null || !models.Any())
                {
                    throw new Exception("No prescriptions found.");
                }

                var viewModel = models
                    .Where(p => p.Appointment.CustomerId == customerId)
                    .Select(p => _mapper.Map<PrescriptionDto>(p))
                    .ToList();

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving prescriptions.", ex);
            }
        }


        public async Task<List<PrescriptionDto>> GetPrescriptions()

        {
            try
            {
                var models = await _preScription.GetPrescriptions();
                if(models == null)
                {
                    throw new Exception("Prescriptions not found");
                }
                return _mapper.Map<List<PrescriptionDto>>(models);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving Prescription.", ex);
            }
        }

        public async Task UpdatePrescription(PrescriptionDto prescription)
        {
            if (prescription == null)
            {
                throw new ArgumentNullException(nameof(prescription), "prescription cannot be null.");
            }

            try
            {
                var model = await _preScription.GetById(prescription.PrescriptionId);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"prescription with ID {prescription.PrescriptionId} not found.");
                }
                model = _mapper.Map<Prescription>(prescription);
                await _preScription.UpdatePrescription(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the prescription.", ex);
            }
        }

        public async Task<List<PrescriptionDto>> GetByAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new ArgumentException("Invalid prescription ID.", nameof(appointmentId));
            }

            try
            {
                var appointment = await _appointmentRepo.GetAppointmentById(appointmentId);
                if (appointment == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Appointment with ID {appointment} not found.");
                }
                var models = await _preScription.GetPrescriptions();
                var prescriptions = models.Where(x=>x.AppointmentId == appointment.AppointmentId).ToList();
                var viewModels = _mapper.Map<List<PrescriptionDto>>(prescriptions);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the Prescription.", ex);
            }
        }

        public async Task UpdatePrescriptionPrice(int prescriptionId)
        {
            var prescription = await _preScription.GetById(prescriptionId);
            if (prescription != null)
            {
                List<PrescriptionMedicine> prescriptionMedicines = prescription.PrescriptionMedicines.ToList();
                if (prescriptionMedicines != null)
                {
                    prescription.Total = 0;
                    foreach (var medicine in prescriptionMedicines)
                    {
                        prescription.Total += medicine.Quantity * medicine.Price;
                    }
                }

                await _preScription.UpdatePrescription(prescription);
            } else throw new ArgumentException("Prescription not found.");

            
        }
    }
}
