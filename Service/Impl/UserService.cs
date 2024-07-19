using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IMapper _mapper;
        public UserService(IUserRepo repo, IRoleRepo roleRepo, IMapper mapper)
        {
            _userRepo = repo;
            _roleRepo = roleRepo;
            _mapper = mapper;
        }
        
        public async Task CreateUser(UserDto user)
        { 
            try
            {
                if (user == null) { throw new ArgumentNullException(nameof(user)); }
                User? model = _mapper.Map<User>(user);  
                await _userRepo.CreateUser(model);  
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

        public async Task DeleteUser(UserDto user)
        {
            try
            {
                var model = await _userRepo.GetById(user.UserId);
                if(model == null)
                {
                    throw new ArgumentException("Not found user");
                }
                 await _userRepo.DeleteUser(model);
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error while delete user", ex);
            }
        }

        public async Task<List<UserDto>?> GetAllDentists()
        {
            try
            {
                var models = await _userRepo.GetAllDentists();
                if (models == null)
                {
                    throw new ArgumentException("Not found users");
                }
                var viewModels  = _mapper.Map<List<UserDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error while delete user", ex);
            }
            
        }

        public async Task<List<UserDto>?> GetAllDentistsByService(int serviceId)
        {
            try
            {
                var models = await _userRepo.GetAllDentistsByService(serviceId);
                if (models == null)
                {
                    throw new ArgumentException("Not found users");
                }
                var viewModels = _mapper.Map<List<UserDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error while delete user", ex);
            }

        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                var models = await _userRepo.GetAllUsers();
                if (models == null)
                {
                    throw new ArgumentException("Not found users");
                }
                var viewModels = _mapper.Map<List<UserDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

        public  async Task<List<UserDto>> GetAllUserByType(string type)
        {
            List<User> models = new List<User>();
            
            try
            {
                switch (type)
                {
                    case "All":
                        models = _userRepo.GetAllUsers().Result;
                        break;
                    case "Dentist":
                        models = await _userRepo.GetAllDentists();
                        break;
                    case "Customer":
                        models = await _userRepo.GetAllCustomer();
                        break;
                    default:
                        models = await _userRepo.GetAllUsers();
                        break;
                }
                if(models == null)
                {
                    throw new ArgumentException("not found users");
                }
                var viewModels = _mapper.Map<List<UserDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error while delete user", ex);
            }
        }

        public async Task<UserDto> GetById(int id)
        {
            if(id == null)
            {
                throw new ExceptionHandler.ServiceException("id not found");
            }
            try
            {
                var model = await _userRepo.GetById(id);
                var viewModel = _mapper.Map<UserDto>(model);
                return viewModel;
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

        public async Task UpdateUser(UserDto user)
        {
            try
            {
                User? models = await _userRepo.GetById(user.UserId);
                if(models != null)
                {
                    await _userRepo.UpdateUser(models);
                } throw new Exception("User not found!");
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

        public async Task<List<UserDto>> GetAllCustomers()
        {
            try
            {
                var models = await _userRepo.GetAllCustomer();
                var viewModels = _mapper.Map<List<UserDto>>(models);  
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }

        public async Task<List<RoleDto>> GetAllRoles()
        {
            try
            {
                var models = await _roleRepo.GetAll();
                var viewModels = _mapper.Map<List<RoleDto>>(models);
                return viewModels;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ExceptionHandler.ServiceException("An error occurred while retrieving", ex);
            }
        }
    }
}
