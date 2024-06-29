using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class DentistServiceRepo : IDentistServiceRepo
    {
        public List<Service> GetAllServiceByDentist(int dentistId) => DentistServiceDAO.Instance.getAllServiceByDentist(dentistId);
    }
}
