using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository.Impl;
using Service.Exeption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public interface IPrescriptionMedicinesService
    {
        Task<List<PrescriptionMedicineDto>> GetAllPrescriptionMedicines();
        Task<List<PrescriptionMedicineDto>> GetAllPrescriptionMedicinesByPrescriptionId( int preId);
        Task<PrescriptionMedicineDto> GetById(int? id);
        Task AddPrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine);
        Task DeletePrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine);
        Task UpdatePrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine);
    }
    public class PrescriptionMedicinesService : IPrescriptionMedicinesService
    {
        private readonly IPrescriptionMedicineRepo _prescriptionMedicineRepo;
        private readonly IMapper _mapper;
        public PrescriptionMedicinesService(IPrescriptionMedicineRepo prescriptionMedicineRepo, IMapper mapper)
        {
            _prescriptionMedicineRepo = prescriptionMedicineRepo;
            _mapper = mapper;
        }
        public async Task UpdatePrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                var model = await _prescriptionMedicineRepo.GetById(prescriptionMedicine.PrescriptionMedicineId);
                if (model == null)
                {
                    throw new ArgumentException("PrescriptionMedicine not found");
                }
                model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                await _prescriptionMedicineRepo.UpdatePrescriptionMedicine(model);


            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating.", ex);
            }

        }
        public async Task AddPrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                var model = await _prescriptionMedicineRepo.GetById(prescriptionMedicine.PrescriptionMedicineId);
                if (model != null)
                {
                    throw new ArgumentException("PrescriptionMedicine exist yet");
                }
                model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                await _prescriptionMedicineRepo.AddPrescriptionMedicine(model);

            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task<List<PrescriptionMedicineDto>> GetAllPrescriptionMedicinesByPrescriptionId(int preId)
        {
            
            try
            {
                var models = await _prescriptionMedicineRepo.GetAllPrescriptionMedicinesByPrescriptionId(preId);
                var viewModels = _mapper.Map<List<PrescriptionMedicineDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }

        public async Task<PrescriptionMedicineDto> GetById(int? id)
        {

            try
            {
                var model = await _prescriptionMedicineRepo.GetById(id);
                var viewModel = _mapper.Map<PrescriptionMedicineDto>(model);
                return viewModel;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }

        public async Task DeletePrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                var model = await _prescriptionMedicineRepo.GetById(prescriptionMedicine.PrescriptionMedicineId);
                if (model == null)
                {
                    throw new ArgumentException("PrescriptionMedicine not found");
                }
                model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                await _prescriptionMedicineRepo.DeletePrescriptionMedicine(model);

            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task<List<PrescriptionMedicineDto>> GetAllPrescriptionMedicines()
        {
            try
            {
                var models = await _prescriptionMedicineRepo.GetAllPrescriptionMedicines();
                var viewModels = _mapper.Map<List<PrescriptionMedicineDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving.", ex);
            }
        }
    }
}
