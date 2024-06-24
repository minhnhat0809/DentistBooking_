using BusinessObject;
using BusinessObject.DTO;
using Repository;

namespace Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo repo)
        {
            _userRepo = repo;
        }
        public void CreateUser(User user)
        => _userRepo.CreateUser(user);

        public void DeleteUser(User id)
        => _userRepo.DeleteUser(id);

        public async Task<List<User>?> GetAllDentists()
        {
            List<User> userList = await _userRepo.GetAllDentists();
            return userList;
        }

        public async Task<List<User>?> GetAllDentistsByService(int serviceId)
        {
            List<User> userList = await _userRepo.GetAllDentistsByService(serviceId);
            

            return userList;
        }

        public List<User> GetAllUsers()
        => _userRepo.GetAllUsers();

        public User GetById(int id)
        => _userRepo.GetById(id);

        public async Task<ResponseDTO> Login(string email, string password)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                User? user = await _userRepo.GetUserByUserName(email);
               
                if (user == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "No account with this email!";
                    return responseDTO;
                }
                else
                {
                    if (user.Password.Equals(password))
                    {
                        responseDTO.IsSuccess = true;
                        responseDTO.Message = "Create successfully !!!";
                        responseDTO.Result = user.Role.RoleName;
                        return responseDTO;
                    }
                    else
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message = "Wrong password !!!";
                        return responseDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }            
        }

        public void UpdateUser(User user)
        => _userRepo.UpdateUser(user);
    }
}
