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
        public async Task<List<Service>> GetAllServiceByDentist(int dentistId) => await DentistServiceDAO.Instance.getAllServiceByDentist(dentistId);
        
        public async Task<List<Service>> GetAllServiceByDentistActive(int dentistId) => await DentistServiceDAO.Instance.getAllServiceByDentistActive(dentistId);

    }
}
