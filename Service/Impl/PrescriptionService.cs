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
        private readonly IMapper _mapper;

        public PrescriptionService(IPrescriptionrepo prescription, IMapper mapper)
        {
            _preScription = prescription ?? throw new ArgumentNullException(nameof(prescription));
            _mapper = mapper;
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

        public async Task DeletePrescription(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid prescription ID.", nameof(id));
            }

            try
            {
                var model = await _preScription.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Prescription with ID {id} not found.");
                }

                await _preScription.DeletePrescription(model.PrescriptionId);
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
            if(customerId == 0)
            {
                throw new Exception("user not found");
            }
            try
            {
                var models = await _preScription.GetPrescriptions();
                if (models == null)
                {
                    throw new Exception("Prescriptions not found");
                }
                var viewModel = _mapper.Map<List<PrescriptionDto>>(models);
                return viewModel.Where(s => s.Appointment.CustomerId == customerId).ToList(); 
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving Prescription.", ex);
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

        
    }
}
