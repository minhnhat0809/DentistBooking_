using BusinessObject;

namespace Repository
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetById(int? id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(User id);

        Task<User?> GetUserByUserName(string email);

        Task<List<User>> GetAllDentists();
        Task<List<User>> GetAllCustomer(); 
        Task<List<User>> GetAllDentistsByService(int serviceId);
    }
}
