using BusinessObject;
using Repository;
using Service.Lib;

namespace Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo repo)
        {
            _userRepo = repo;
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

        public List<User> GetAllUsers()
        => _userRepo.GetAllUsers();

        public User GetById(int id)
        => _userRepo.GetById(id);

        public void UpdateUser(User user)
        {
            if (!IsValidUser(user))
            {
                throw new ArgumentException("Invalid user data. Please check all fields.");
            }
             _userRepo.UpdateUser(user);
        }
          
    }
}
