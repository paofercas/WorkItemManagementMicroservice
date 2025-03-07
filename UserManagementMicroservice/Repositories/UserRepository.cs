using System.Collections.Generic;
using System.Linq;
using UserManagementMicroservice.Data;
using UserManagementMicroservice.Models;

namespace UserManagementMicroservice.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void UpdateUser(User user, string username)
        {
            var existing = GetUserByUsername(username);
            if (existing != null)
            {
                existing.HighRelevanceCount = user.HighRelevanceCount;
                existing.PendingItemsCount = user.PendingItemsCount;
                _context.SaveChanges();
            }
        }
    }
}
