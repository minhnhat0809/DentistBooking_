using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public interface IRoleRepo
    {
        Task<List<Role>> GetAll();
        Task<Role> GetById(int? id);
        Task Create(Role schedule);
        Task Update(Role schedule);
        Task Delete(int id);

    }
    public class RoleRepo : IRoleRepo
    {
        public async Task Create(Role schedule)
        => await RoleDAO.Instance.create(schedule);

        public async Task Delete(int id)
        { 
            var model = await RoleDAO.Instance.getByID(id);
            await RoleDAO.Instance.delete(model);
        }
        public async Task<List<Role>> GetAll()
        => await RoleDAO.Instance.getAll();

        public async Task<Role> GetById(int? id)
        => await RoleDAO.Instance.getByID(id);

        public async Task Update(Role schedule)
        => await RoleDAO.Instance.update(schedule);
    }
}
