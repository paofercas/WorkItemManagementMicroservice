using UserManagementMicroservice.Models;
using UserManagementMicroservice.Repositories;

namespace UserManagementMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public void UpdateUser(User user, string username)
        {
            _userRepository.UpdateUser(user, username);
        }
    }
}
