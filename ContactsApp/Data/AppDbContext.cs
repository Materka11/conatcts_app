using ContactsApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

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

            //ustawienie relacji jeden -do-wielu między tabelą Contacts a tabelą Categories
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Contacts)
                .HasForeignKey(c => c.CategoryId);

            //ustawienie relacji jeden -do-wielu między tabelą Contacts a tabelą Subcategories
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Subcategory)
                .WithMany(s => s.Contacts)
                .HasForeignKey(c => c.SubcategoryId);
        }
    }
}
