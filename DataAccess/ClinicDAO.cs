using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ClinicDAO
    {
        private static ClinicDAO instance = null;
        private static readonly object instanceLock = new object();
        private ClinicDAO() { }
        public static ClinicDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ClinicDAO();
                    }
                    return instance;
                }
            }
        }
    }
}
