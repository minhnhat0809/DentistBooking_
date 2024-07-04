using BusinessObject;
using DataAccess;

namespace Repository.Impl
{
    public class UserRepo : IUserRepo
    {
        public void CreateUser(User user)
        => UserDAO.Instance.createUser(user);

        public void DeleteUser(User id)
        => UserDAO.Instance.deleteUser(id);

        public async Task<List<User>> GetAllDentists() => await UserDAO.Instance.GetAllDentists();

        public async Task<List<User>> GetAllDentistsByService(int serviceId) => await UserDAO.Instance.GetAllDentistsByService(serviceId);

        public List<User> GetAllUsers()
        => UserDAO.Instance.getAllUsers();

        public User GetById(int? id)
        => UserDAO.Instance.getUserByID(id);

        public async Task<User?> GetUserByUserName(string email) => await UserDAO.Instance.GetUserByUserName(email);

        public void UpdateUser(User user)
        => UserDAO.Instance.updateUser(user);
    }
}
