using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MedicineDAO
    {
        private static MedicineDAO instance = null;
        private static readonly object instanceLock = new object();
        private MedicineDAO() { }
        public static MedicineDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MedicineDAO();
                    }
                    return instance;
                }
            }
        }
    }
}
