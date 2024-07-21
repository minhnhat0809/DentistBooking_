using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Result;

namespace Service
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetById(int id);
        Task CreateUser(UserDto user);
        Task UpdateUser(UserDto user);
        Task DeleteUser(UserDto id);

        Task<ResponseDTO> Login(string userName, string password);
        Task<List<UserDto>?> GetAllDentists();
        Task<List<UserDto>> GetAllCustomers();
        Task<List<UserDto>?> GetAllDentistsByService(int serviceId);
        Task<List<RoleDto>> GetAllRoles();
        Task<List<UserDto>> GetAllUserByType(string type);
        Task<UserDto> GetCustomerByPhoneNumber(string phoneNumber);

        
        Task<ListUserResult> GetAllActiveCustomers();

    }
}
