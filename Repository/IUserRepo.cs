using BusinessObject;

namespace Repository
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();
        public User GetById(int? id);
        public void CreateUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(User id);

        Task<User?> GetUserByUserName(string email);

        Task<List<User>> GetAllDentists();

        Task<List<User>> GetAllDentistsByService(int serviceId);
    }
}
