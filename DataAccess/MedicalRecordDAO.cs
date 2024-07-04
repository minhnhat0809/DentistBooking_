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

        public async Task<MedicalRecord> getMedicalRecordByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var medicalRecord = await context.MedicalRecords
                .Include(x=>x.Customer)
                .FirstOrDefaultAsync(c => c.MediaRecordId == id);
            return medicalRecord;
        }

        public async Task<List<MedicalRecord>> getAllMedicalRecords()
        {
            var context = new BookingDentistDbContext();
            var medicalRecordList = await context.MedicalRecords
                .Include(x=>x.Customer)
                .ToListAsync();
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
