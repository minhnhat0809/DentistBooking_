using BusinessObject;
using BusinessObject.DTO;

namespace Service
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsers();
        public Task<UserDto> GetById(int id);
        public void CreateUser(UserDto user);
        public void UpdateUser(UserDto user);
        public void DeleteUser(UserDto id);

        Task<ResponseDTO> Login(string userName, string password);
        Task<List<UserDto>?> GetAllDentists();
        Task<List<UserDto>> GetAllCustomers();
        Task<List<UserDto>?> GetAllDentistsByService(int serviceId);
        Task<List<RoleDto>> GetAllRoles();
        Task<List<UserDto>> GetAllUserByType(string type);

    }
}
