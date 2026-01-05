using Microsoft.EntityFrameworkCore;
using UserService.Core.Models;

namespace UserService.Infrastructure.Persistence
{
    public class UserDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasKey(u => u.Email);

            modelBuilder.Entity<AppUser>().Property(u => u.Email).IsRequired();

            modelBuilder.Entity<AppUser>().Property(u => u.Name).IsRequired();
        }
    }
}
