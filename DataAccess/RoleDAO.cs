using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoleDAO
    {
        private static RoleDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoleDAO() { }
        public static RoleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoleDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Role>> getAll()
        {
            var context = new BookingDentistDbContext();
            var roles = await context.Roles
                .ToListAsync();
            return roles;
        }
        public async Task<Role> getByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var role = await context.Roles
                .FirstOrDefaultAsync(c => c.RoleId == id);
            return role;
        }


        public void delete(Role role)
        {
            var context = new BookingDentistDbContext();
            context.Roles .Remove(role); 
            context.SaveChanges();
        }

        public void create(Role role)
        {
            var context = new BookingDentistDbContext();
            context.Roles.Add(role);
            context.SaveChanges();
        }

        public void update(Role role)
        {
            var context = new BookingDentistDbContext();
            context.Entry<Role>(role).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
