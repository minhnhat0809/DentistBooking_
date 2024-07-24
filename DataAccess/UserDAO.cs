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

        public async Task<User> getUserByID(int? id)
        {
            var context = new BookingDentistDbContext();
            var user = await context.Users
                .Include(x => x.Clinic)
                .Include(x => x.Role)
                .Include(x => x.MedicalRecords)
                .FirstOrDefaultAsync(c => c.UserId == id);
            return user;
        }

        public async Task<List<User>> getAllUsers()
        {
            var context = new BookingDentistDbContext();
            var userList = await context.Users
                .Include(x => x.Role)
                .Include(x => x.Clinic)
                .Where(c => c.RoleId != 1)
                .ToListAsync();
            return userList;
        }

        public async Task<List<User>> GetAllCustomer()
        {
            var context = new BookingDentistDbContext();
            var userList = await context.Users
                .Include(x => x.Role)
                .Include(x => x.Clinic)
                .Where(u => u.Role.RoleName.Equals("Customer"))
                .ToListAsync();
            return userList;
        }

        public async Task<User> GetCustomerByPhoneNumber(string phoneNumber)
        {
            var context = new BookingDentistDbContext();
            var customer = await context.Users
                .Include(x => x.Role)
                .Include(x => x.Clinic)
                .Where(u => u.Role.RoleName.Equals("Customer") && u.PhoneNumber.Equals(phoneNumber))
                .FirstOrDefaultAsync();
            return customer;
        }

        public async Task deleteUser(User user)
        {
            var context = new BookingDentistDbContext();
            user.Status = false;
            context.Entry<User>(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task createUser(User user)
        {
            var context = new BookingDentistDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task updateUser(User user)
        {
            var context = new BookingDentistDbContext();
            context.Entry<User>(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByUserName(string email)
        {
            var context = new BookingDentistDbContext();
            User? user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(c => c.Email.Equals(email));
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
            var dentistService = context.DentistServices
                .Include(ds => ds.Dentist)
                .Where(ds => ds.ServiceId == serviceId)
                .ToList();
            dentistService.Where(ds => ds.Status == true).ToList();

            var dentistList = dentistService.Select(ds => ds.Dentist).ToList();
            return dentistList;
        }



    }
}
