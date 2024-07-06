using BusinessObject;
using BusinessObject.DTO;
using Repository;
using Repository.Impl;
using Service.Exeption;
using Service.Lib;

namespace Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        public UserService(IUserRepo repo, IRoleRepo roleRepo)
        {
            _userRepo = repo;
            _roleRepo = roleRepo;
        }
        private bool IsValidUser(User user)
        {
            return user != null &&
                   (Validation.ValidateUserName(user.UserName) &&
                    Validation.ValidateName(user.Name) &&
                    Validation.ValidatePassword(user.Password) &&                  
                    Validation.ValidatePhoneNumber(user.PhoneNumber) &&
                    Validation.ValidateEmail(user.Email));
        }
        public void CreateUser(User user)
        {    
            if (!IsValidUser(user))
            {
                throw new ArgumentException("Invalid user data. Please check all fields.");
            }

            _userRepo.CreateUser(user);
        }

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

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var models = await _userRepo.GetAllUsers();
                
                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }
        public  List<User> GetAllUserByType(string type)
        {
            List<User> userList = new List<User>();
            switch (type)
            {
                case "All":
                    userList =   _userRepo.GetAllUsers().Result;
                    break;
                case "Dentist":
                    userList =  _userRepo.GetAllDentists().Result;   
                    break;
                case "Customer":
                    userList =  _userRepo.GetAllCustomer().Result;
                    break;
                default:
                    userList =  _userRepo.GetAllUsers().Result;
                    break;
            }
            return userList;
        }

        

        public async Task<User> GetById(int id)
        {
            if(id == null)
            {
                throw new ExceptionHandler.ServiceException("id not found");
            }
            try
            {
                var models = await _userRepo.GetById(id);

                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

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
                        responseDTO.Message = user.UserId.ToString();
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
        {
            if (!IsValidUser(user))
            {
                throw new ArgumentException("Invalid user data. Please check all fields.");
            }
             _userRepo.UpdateUser(user);
        }

        public async Task<List<User>> GetAllCustomers()
        {
            try
            {
                var models = await _userRepo.GetAllCustomer();

                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var models = await _roleRepo.GetAll();

                return models;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }
    }
}
