using System;
using System.Collections.Generic;
using BusinessObject;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepo _clinicRepo;

        public ClinicService(IClinicRepo clinicRepo)
        {
            _clinicRepo = clinicRepo ?? throw new ArgumentNullException(nameof(clinicRepo));
        }

        public List<Clinic> GetAllClinics()
        {
            try
            {
                return _clinicRepo.GetAllClinics();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving clinics.", ex);
            }
        }

        public Clinic GetById(int id)
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

                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the clinic.", ex);
            }
        }

        public void CreateClinic(Clinic clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentNullException(nameof(clinic), "Clinic cannot be null.");
            }

            try
            {
                var existingClinic = _clinicRepo.GetById(clinic.ClinicId);
                if (existingClinic != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {clinic.ClinicId} already exists.");
                }

                _clinicRepo.CreateClinic(clinic);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the clinic.", ex);
            }
        }

        public void UpdateClinic(Clinic clinic)
        {
            if (clinic == null)
            {
                throw new ArgumentNullException(nameof(clinic), "Clinic cannot be null.");
            }

            try
            {
                var existingClinic = _clinicRepo.GetById(clinic.ClinicId);
                if (existingClinic == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Clinic with ID {clinic.ClinicId} not found.");
                }

                _clinicRepo.UpdateClinic(clinic);
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
                var existingClinic = _clinicRepo.GetById(id);
                if (existingClinic == null)
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
