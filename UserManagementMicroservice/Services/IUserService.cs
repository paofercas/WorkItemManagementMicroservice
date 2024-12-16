using UserManagementMicroservice.Models;

namespace UserManagementMicroservice.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByUsername(string username);
        void UpdateUser(User user, string username);
    }
}
