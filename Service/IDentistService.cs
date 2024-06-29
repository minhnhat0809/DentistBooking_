using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDentistService
    {
        List<BusinessObject.Service> GetAllServiceByDentist(int dentistId, int serviceId);
    }
}
