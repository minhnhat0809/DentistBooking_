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

        public async Task CreateClinic(ClinicDto clinic)
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

                // Map the valid ClinicDto to Clinic entity
                model = _mapper.Map<Clinic>(clinic);

                // Create the clinic in the repository
                await _clinicRepo.CreateClinic(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the clinic.", ex);
            }
        }

        public async Task UpdateClinic(ClinicDto clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentNullException(nameof(clinic), "Clinic cannot be null.");
            }

            // Validate the ClinicDto object
            ValidateClinicInfo(clinic);

            try
            {
                var existingClinic = await _clinicRepo.GetById(clinic.ClinicId);
                if (existingClinic == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Clinic with ID {clinic.ClinicId} not found.");
                }

                // Map the valid ClinicDto to Clinic entity
                var model = _mapper.Map<Clinic>(clinic);

                // Update the clinic in the repository
                await _clinicRepo.UpdateClinic(model);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the clinic.", ex);
            }
        }

        private void ValidateClinicInfo(ClinicDto clinicDto)
        {
            // Validate ClinicName
            if (string.IsNullOrWhiteSpace(clinicDto.ClinicName))
            {
                throw new ExceptionHandler.ServiceException("Clinic Name is required.");
            }
            else if (clinicDto.ClinicName.Length > 100)
            {
                throw new ExceptionHandler.ServiceException("Clinic Name can't be longer than 100 characters.");
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(clinicDto.Address))
            {
                throw new ExceptionHandler.ServiceException("Address is required.");
            }
            else if (clinicDto.Address.Length > 200)
            {
                throw new ExceptionHandler.ServiceException("Address can't be longer than 200 characters.");
            }

            // Validate Phone
            if (!string.IsNullOrWhiteSpace(clinicDto.Phone))
            {
                // A basic phone number validation (this can be customized)
                if (!System.Text.RegularExpressions.Regex.IsMatch(clinicDto.Phone, @"^\+?[1-9]\d{1,14}$"))
                {
                    throw new ExceptionHandler.ServiceException("Invalid Phone number format.");
                }
            } else throw new ExceptionHandler.ServiceException("Phone is required.");

            // Validate Email
            if (!string.IsNullOrWhiteSpace(clinicDto.Email))
            {
                try
                {
                    var emailAddress = new System.Net.Mail.MailAddress(clinicDto.Email);
                }
                catch
                {
                    throw new ExceptionHandler.ServiceException("Invalid Email address format.");
                }
            } else throw new ExceptionHandler.ServiceException("Phone is required.");
        }


        public async Task DeleteClinic(int id)
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

                await _clinicRepo.DeleteClinic(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the clinic.", ex);
            }
        }
    }
}
