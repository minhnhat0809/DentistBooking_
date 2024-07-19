using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class UserRepo : IUserRepo
    {
        public async Task CreateUser(User user)
        => await UserDAO.Instance.createUser(user);

        public async Task DeleteUser(User id)
        => await UserDAO.Instance.deleteUser(id);

        public async Task<List<User>> GetAllCustomer()
        => await UserDAO.Instance.GetAllCustomer();


        public async Task<List<User>> GetAllDentists() => await UserDAO.Instance.GetAllDentists();

        public async Task<List<User>> GetAllDentistsByService(int serviceId) => await UserDAO.Instance.GetAllDentistsByService(serviceId);

        public async Task<List<User>> GetAllUsers()
        => await UserDAO.Instance.getAllUsers();

        public async Task<User> GetById(int? id)
        => await UserDAO.Instance.getUserByID(id);

        public async Task<User?> GetUserByUserName(string email) => await UserDAO.Instance.GetUserByUserName(email);

        public async Task UpdateUser(User user)
        => await UserDAO.Instance.updateUser(user);
    }
}
