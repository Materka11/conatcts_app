using ContactsApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
