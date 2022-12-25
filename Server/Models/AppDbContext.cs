using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated(); 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestSystemDb;Trusted_Connection=True;");
        }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
