using Microsoft.EntityFrameworkCore;
using UserManagementMicroservice.Models;

namespace UserManagementMicroservice.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Username); // primary key Username

            //Creating the first elements for the database.
            modelBuilder.Entity<User>().HasData(
                new User { Username = "UsuarioA", HighRelevanceCount = 2, PendingItemsCount = 3 },
                new User { Username = "UsuarioB", HighRelevanceCount = 0, PendingItemsCount = 1 },
            );
        }
    }
}
