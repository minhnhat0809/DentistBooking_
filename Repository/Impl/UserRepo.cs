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

        public List<User> GetAllUsers()
        => UserDAO.Instance.getAllUsers();

        public User GetById(int id)
        => UserDAO.Instance.getUserByID(id);

        public void UpdateUser(User user)
        => UserDAO.Instance.updateUser(user);
    }
}
