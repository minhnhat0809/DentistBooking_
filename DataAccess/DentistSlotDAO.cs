using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DentistSlotDAO
    {
        private static DentistSlotDAO instance = null;
        private static readonly object instanceLock = new object();
        private DentistSlotDAO() { }
        public static DentistSlotDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DentistSlotDAO();
                    }
                    return instance;
                }
            }
        }
    }
}
