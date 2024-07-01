using BusinessObject;

namespace Repository
{
    public interface IPrescriptionrepo
    {
        public List<Prescription> GetPrescriptions();
        public Prescription GetById(int id);
        public void CreatePrescription(Prescription prescription);
        public void UpdatePrescription(Prescription prescription);
        public void DeletePrescription(int id);
    }
}
