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
        public Task<List<Role>> GetAll();
        public Task<Role> GetById(int? id);
        public void Create(Role schedule);
        public void Update(Role schedule);
        public void Delete(int id);

    }
    public class RoleRepo : IRoleRepo
    {
        public  void Create(Role schedule)
        =>  RoleDAO.Instance.create(schedule);

        public async void Delete(int id)
        { 
            var model = await RoleDAO.Instance.getByID(id);
            RoleDAO.Instance.delete(model);
        }
        public async Task<List<Role>> GetAll()
        => await RoleDAO.Instance.getAll();

        public async Task<Role> GetById(int? id)
        => await RoleDAO.Instance.getByID(id);

        public void Update(Role schedule)
        => RoleDAO.Instance.update(schedule);
    }
}
