using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MedicalRecordDAO
    {
        private static MedicalRecordDAO instance = null;
        private static readonly object instanceLock = new object();
        private MedicalRecordDAO() { }
        public static MedicalRecordDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MedicalRecordDAO();
                    }
                    return instance;
                }
            }
        }

        public MedicalRecord getMedicalRecordByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var medicalRecord = context.MedicalRecords.FirstOrDefault(c => c.MediaRecordId == id);
            return medicalRecord;
        }

        public List<MedicalRecord> getAllMedicalRecords()
        {
            var context = new BookingDentistDbContext();
            var medicalRecordList = context.MedicalRecords.ToList();
            return medicalRecordList;
        }

        public void deleteMedicalRecord(MedicalRecord medicalRecord)
        {
            var context = new BookingDentistDbContext();
            medicalRecord.Status = false;
            context.Entry<MedicalRecord>(medicalRecord).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createMedicalRecord(MedicalRecord medicalRecord)
        {
            var context = new BookingDentistDbContext();
            context.MedicalRecords.Add(medicalRecord);
            context.SaveChanges();
        }

        public void updateMedicalRecord(MedicalRecord medicalRecord)
        {
            var context = new BookingDentistDbContext();
            context.Entry<MedicalRecord>(medicalRecord).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
