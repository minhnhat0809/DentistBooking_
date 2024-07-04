using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IUserService
    {
        public List<User> GetAllUsers();
        public User GetById(int id);
        public void CreateUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(User id);

        Task<ResponseDTO> Login(string userName, string password);
        Task<List<User>?> GetAllDentists();

        Task<List<User>?> GetAllDentistsByService(int serviceId);

        List<User> GetAllUserByType(string type);


    }
}
