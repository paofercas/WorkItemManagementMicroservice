using System.Collections.Generic;
using UserManagementMicroservice.Models;

namespace UserManagementMicroservice.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByUsername(string username);
        void UpdateUser(User user, string username);
    }
}
