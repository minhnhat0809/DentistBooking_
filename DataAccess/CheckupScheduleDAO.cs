using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CheckupScheduleDAO
    {
        private static CheckupScheduleDAO instance = null;
        private static readonly object instanceLock = new object();
        private CheckupScheduleDAO() { }
        public static CheckupScheduleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CheckupScheduleDAO();
                    }
                    return instance;
                }
            }
        }
    }
}
