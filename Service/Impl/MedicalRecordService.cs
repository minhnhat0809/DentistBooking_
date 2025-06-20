﻿using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepo _medicalRecordRepo;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepo medicalRecordRepo, IMapper mapper)
        {
            _medicalRecordRepo = medicalRecordRepo ?? throw new ArgumentNullException(nameof(medicalRecordRepo));
            _mapper = mapper;
        }

        public async Task<List<MedicalRecordDto>> GetAllMedicalRecords()
        {
            try
            {
                List<MedicalRecord> models = await _medicalRecordRepo.GetAllMedicalRecords();
                var viewModels = _mapper.Map<List<MedicalRecordDto>>(models);
                
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving medical records.", ex);
            }
        }

        public async Task CreateMedicalRecord(MedicalRecordDto medical)
        {
            if (medical == null)
            {
                throw new ArgumentNullException(nameof(medical), "Medical record cannot be null.");
            }

            try
            {
                var model = await _medicalRecordRepo.GetById(medical.MediaRecordId);


                medical.TimeStart = DateTime.Now;
                medical.Duration = TimeOnly.FromDateTime(medical.TimeStart);
                model = _mapper.Map<MedicalRecord>(medical);
                await _medicalRecordRepo.CreateMedicalRecord(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the medical record.", ex);
            }
        }

        public async Task UpdateMedicalRecord(MedicalRecordDto medical)
        {
            if (medical == null)
            {
                throw new ArgumentNullException(nameof(medical), "Medical record cannot be null.");
            }

            try
            {
                var model = await _medicalRecordRepo.GetById(medical.MediaRecordId);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {medical.MediaRecordId} not found.");
                }
                medical.TimeStart = DateTime.Now;
                medical.Duration = TimeOnly.FromDateTime(medical.TimeStart);
                
                model = _mapper.Map<MedicalRecord>(medical);

                await _medicalRecordRepo.UpdateMedicalRecord(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the medical record.", ex);
            }
        }

        public async Task DeleteMedicalRecord(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var existingRecord = await _medicalRecordRepo.GetById(id);
                if (existingRecord == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {id} not found.");
                }

                await _medicalRecordRepo.DeleteMedicalRecord(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the medical record.", ex);
            }
        }

        public async Task<MedicalRecordDto> GetById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid medical record ID.", nameof(id));
            }

            try
            {
                var model = await _medicalRecordRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record with ID {id} not found.");
                }

                return _mapper.Map<MedicalRecordDto>(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medical record.", ex);
            }
        }

        public async Task<List<MedicalRecordDto>> GetMedicalRecordsByCustomerIdAsync(int customerId)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Invalid medical customer ID.", nameof(customerId));
            }

            try
            {
                var models = await _medicalRecordRepo.GetMedicalRecordsByCustomerIdAsync(customerId);
                var medicalRecords = models.ToList();
                var s = medicalRecords.Where(m => m.CustomerId == customerId).FirstOrDefault();
                medicalRecords.Remove(s);
                medicalRecords.Insert(0, s);
                if (models == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Medical record not found.");
                }
                var viewModels = _mapper.Map<List<MedicalRecordDto>>(medicalRecords);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the medical record.", ex);
            }
        }
    }
}
