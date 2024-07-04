using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();
        private UserDAO() { }
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public User getUserByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var user = context.Users.FirstOrDefault(c => c.UserId == id);
            return user;
        }

        public List<User> getAllUsers()
        {
            var context = new BookingDentistDbContext();
            var userList = context.Users.ToList();
            return userList;
        }

        public void deleteUser(User user)
        {
            var context = new BookingDentistDbContext();
            user.Status = false;
            context.Entry<User>(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void createUser(User user)
        {
            var context = new BookingDentistDbContext();
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void updateUser(User user)
        {
            var context = new BookingDentistDbContext();
            context.Entry<User>(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public async Task<User?> GetUserByUserName(string email)
        {
            var context = new BookingDentistDbContext();
            User? user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(c => c.Email == email);
            return user;
        }

        public async Task<List<User>> GetAllDentists()
        {
            var context = new BookingDentistDbContext();
            var userList = await context.Users.Where(u => u.Role.RoleName.Equals("Dentist")).ToListAsync();
            return userList;
        }

        public async Task<List<User>> GetAllDentistsByService(int serviceId)
        {
            var context = new BookingDentistDbContext();
            var dentistService = context.DentistServices.Include(ds => ds.Dentist).Where(ds => ds.ServiceId == serviceId).ToList();
            dentistService.Where(ds => ds.Status == true).ToList();

            var dentistList = dentistService.Select(ds => ds.Dentist).ToList();
            return dentistList;
        }



    }
}
