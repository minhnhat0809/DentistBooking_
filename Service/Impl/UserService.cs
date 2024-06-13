using BusinessObject;
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

        public List<User> GetAllUsers()
        => _userRepo.GetAllUsers();

        public User GetById(int id)
        => _userRepo.GetById(id);

        public void UpdateUser(User user)
        => _userRepo.UpdateUser(user);
    }
}
