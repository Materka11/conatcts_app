using ContactsApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ustawienie unikalnego indeksu na polu Email w tabeli Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //ustawienie unikalnego indeksu na polu Email w tabeli Conatcts
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
