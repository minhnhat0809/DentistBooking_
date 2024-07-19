using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepo _medicineRepo;
        private readonly IMapper _mapper;

        public MedicineService(IMedicineRepo medicineRepo, IMapper mapper)
        {
            _medicineRepo = medicineRepo ?? throw new ArgumentNullException(nameof(medicineRepo));
            _mapper = mapper;
        }

        public async Task<List<MedicineDto>> GetAllMedicines()
        {
            try
            {
                var models = await _medicineRepo.GetAllMedicines();
                return _mapper.Map<List<MedicineDto>>(models);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving medicines.", ex);
            }
        }

        public async Task<MedicineDto> GetById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medicine ID.", nameof(id));
            }

            try
            {
                var model = await _medicineRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {id} not found.");
                }
                var viewModel = _mapper.Map<MedicineDto>(model);
                return viewModel;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medicine.", ex);
            }
        }

        public async Task CreateMedicine(MedicineDto medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(medicine), "Medicine cannot be null.");
            }

            try
            {
                var model = await _medicineRepo.GetById(medicine.MedicineId);
                if (model != null)
                {
                    throw new InvalidOperationException($"Medicine with ID {medicine.MedicineId} already exists.");
                }
                model = _mapper.Map<Medicine>(medicine);
                await _medicineRepo.CreateMedicine(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the medicine.", ex);
            }
        }

        public async Task UpdateMedicine(MedicineDto medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(medicine), "Medicine cannot be null.");
            }

            try
            {
                var model =await _medicineRepo.GetById(medicine.MedicineId);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {medicine.MedicineId} not found.");
                }
                model = _mapper.Map<Medicine>(medicine);
                await _medicineRepo.UpdateMedicine(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the medicine.", ex);
            }
        }

        public async Task DeleteMedicine(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medicine ID.", nameof(id));
            }

            try
            {
                var existingMedicine = await _medicineRepo.GetById(id);
                if (existingMedicine == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medicine with ID {id} not found.");
                }

                await _medicineRepo.DeleteMedicine(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the medicine.", ex);
            }
        }
    }
}
