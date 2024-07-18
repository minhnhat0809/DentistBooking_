using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepo _clinicRepo;
        private readonly IMapper _mapper;
        public ClinicService(IClinicRepo clinicRepo, IMapper mapper)
        {
            _clinicRepo = clinicRepo ?? throw new ArgumentNullException(nameof(clinicRepo));
            _mapper = mapper;
        }

        public async Task<List<ClinicDto>> GetAllClinics()
        {
            try
            {
                var models = await _clinicRepo.GetAllClinics();
                return _mapper.Map<List<ClinicDto>>(models);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving clinics.", ex);
            }
        }

        public async Task<ClinicDto> GetById(int? id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid clinic ID.", nameof(id));
            }

            try
            {
                var model = await _clinicRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Clinic with ID {id} not found.");
                }

                return _mapper.Map<ClinicDto>(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the clinic.", ex);
            }
        }

        public async void CreateClinic(ClinicDto clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentNullException(nameof(clinic), "Clinic cannot be null.");
            }
            try
            {
                var model = await _clinicRepo.GetById(clinic.ClinicId);
                if (model != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {clinic.ClinicId} already exists.");
                }
                model = _mapper.Map<Clinic>(clinic); 
                _clinicRepo.CreateClinic(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the clinic.", ex);
            }
        }

        public async void UpdateClinic(ClinicDto clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentNullException(nameof(clinic), "Clinic cannot be null.");
            }

            try
            {
                var model = await _clinicRepo.GetById(clinic.ClinicId);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Clinic with ID {clinic.ClinicId} not found.");
                }
                model = _mapper.Map<Clinic>(clinic);
                _clinicRepo.UpdateClinic(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the clinic.", ex);
            }
        }

        public void DeleteClinic(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid clinic ID.", nameof(id));
            }

            try
            {
                var model = _clinicRepo.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Clinic with ID {id} not found.");
                }

                _clinicRepo.DeleteClinic(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the clinic.", ex);
            }
        }
    }
}
