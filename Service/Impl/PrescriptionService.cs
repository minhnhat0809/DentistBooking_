using BusinessObject;
using Repository;
using Service.Exeption;

namespace Service.Impl
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionrepo _preScription;

        public PrescriptionService(IPrescriptionrepo prescription)
        {
            _preScription = prescription ?? throw new ArgumentNullException(nameof(prescription));
        }
        public void CreatePrescription(Prescription prescription)
        {
            if (prescription == null)
            {
                throw new ArgumentNullException(nameof(prescription), "Prescription cannot be null.");
            }

            try
            {
                var existingPrescription = _preScription.GetById(prescription.PrescriptionId);
                if (existingPrescription != null)
                {
                    throw new InvalidOperationException($"Medicine with ID {prescription.PrescriptionId} already exists.");
                }

                _preScription.CreatePrescription(prescription);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while creating the prescription.", ex);
            }
        }

        public void DeletePrescription(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid prescription ID.", nameof(id));
            }

            try
            {
                var existingMedicine = _preScription.GetById(id);
                if (existingMedicine == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Prescription with ID {id} not found.");
                }

                _preScription.DeletePrescription(id);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while deleting the prescription.", ex);
            }
        }

        public Prescription GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid prescription ID.", nameof(id));
            }

            try
            {
                var model = _preScription.GetById(id);
                if (model == null)
                {
                    throw new ExceptionHandler.NotFoundException($"Prescription with ID {id} not found.");
                }

                return model;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving the Prescription.", ex);
            }
        }

        public List<Prescription> GetPrescriptions()
        {
            try
            {
                return _preScription.GetPrescriptions();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving Prescription.", ex);
            }
        }

        public void UpdatePrescription(Prescription prescription)
        {
            if (prescription == null)
            {
                throw new ArgumentNullException(nameof(prescription), "prescription cannot be null.");
            }

            try
            {
                var existingMedicine = _preScription.GetById(prescription.PrescriptionId);
                if (existingMedicine == null)
                {
                    throw new ExceptionHandler.NotFoundException($"prescription with ID {prescription.PrescriptionId} not found.");
                }

                _preScription.UpdatePrescription(prescription);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while updating the prescription.", ex);
            }
        }
    }
}
