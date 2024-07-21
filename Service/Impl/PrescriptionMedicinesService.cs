using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
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
        private readonly IPrescriptionrepo _prescriptionRepo;
        private readonly IMapper _mapper;
        private readonly IMedicineRepo _medicineRepo;
        public PrescriptionMedicinesService(IPrescriptionMedicineRepo prescriptionMedicineRepo, IMapper mapper, IPrescriptionrepo prescriptionRepo, IMedicineRepo medicineRepo)
        {
            _prescriptionMedicineRepo = prescriptionMedicineRepo;
            _mapper = mapper;
            _prescriptionRepo = prescriptionRepo;
            _medicineRepo = medicineRepo;
        }
        public async Task UpdatePrescriptionMedicine(PrescriptionMedicineDto prescriptionMedicine)
        {
            if (prescriptionMedicine == null)
            {
                throw new ArgumentNullException(nameof(prescriptionMedicine), "prescriptionMedicine cannot be null.");
            }

            try
            {
                var existingPrescriptionMedicine = await _prescriptionMedicineRepo.GetById(prescriptionMedicine.PrescriptionMedicineId);
                if (existingPrescriptionMedicine != null)
                {
                    // Revert the old medicine quantity back to stock
                    var oldMedicine = await _medicineRepo.GetById(existingPrescriptionMedicine.MedicineId);
                    if (oldMedicine != null)
                    {
                        oldMedicine.Quantity += existingPrescriptionMedicine.Quantity;
                        await _medicineRepo.UpdateMedicine(oldMedicine);
                    }

                    // Deduct new medicine quantity from stock
                    var newMedicine = await _medicineRepo.GetById(prescriptionMedicine.MedicineId);
                    if (newMedicine == null || newMedicine.Quantity < prescriptionMedicine.Quantity)
                    {
                        throw new InvalidOperationException("Not enough medicine in stock.");
                    }
                    newMedicine.Quantity -= prescriptionMedicine.Quantity;
                    await _medicineRepo.UpdateMedicine(newMedicine);

                    // Update the PrescriptionMedicine record
                    var model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                    await _prescriptionMedicineRepo.UpdatePrescriptionMedicine(model);


                } else throw new ArgumentException("PrescriptionMedicine does not exist.");

            }
            catch (Exception ex)
            {
                throw new ExceptionHandler.ServiceException("An error occurred while updating the prescription medicine.", ex);
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
                var existingPrescriptionMedicine = await _prescriptionMedicineRepo.GetById(prescriptionMedicine.PrescriptionMedicineId);
                if (existingPrescriptionMedicine == null)
                {
                    var medicine = await _medicineRepo.GetById(prescriptionMedicine.MedicineId);
                    if (medicine != null && medicine.Quantity > prescriptionMedicine.Quantity)
                    {
                        var prescription = await _prescriptionRepo.GetById(prescriptionMedicine.PrescriptionId.Value);
                        if (prescription != null)
                        {
                            foreach (var medicinePrescription in prescription.PrescriptionMedicines)
                            {
                                if(medicinePrescription.MedicineId == prescriptionMedicine.MedicineId)
                                {
                                    throw new ArgumentException("This medicine have in Prescription already!");
                                }
                            }
                            // Deduct quantity from stock
                            medicine.Quantity -= prescriptionMedicine.Quantity;
                            await _medicineRepo.UpdateMedicine(medicine);

                            // if there no any exit name then add new medicine
                            var model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                            await _prescriptionMedicineRepo.AddPrescriptionMedicine(model);

                        }

                    } else throw new ArgumentNullException("Not enough medicine in stock.");

                } else throw new ArgumentException("PrescriptionMedicine already exists.");

                
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception(ex.Message, ex);
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
                // update medicines storage
                var medicine = await _medicineRepo.GetById(prescriptionMedicine.MedicineId);
                if (medicine != null)
                {
                    medicine.Quantity += prescriptionMedicine.Quantity;
                    await _medicineRepo.UpdateMedicine(medicine);
                }
                model = _mapper.Map<PrescriptionMedicine>(prescriptionMedicine);
                await _prescriptionMedicineRepo.DeletePrescriptionMedicine(model);

                // Update the prescription total price
                await _prescriptionRepo.UpdatePrescription(model.Prescription);
            }
            catch (Exception ex)
            {
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
