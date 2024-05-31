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
    }
}
