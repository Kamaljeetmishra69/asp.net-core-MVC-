using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using udemy.Models;


namespace udemy.data
{
    public class ApplicationDbContext : DbContext 

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>option) : base(option)
        {
          
        }
        public DbSet<udemy.Models.Category> Modifiedcategories { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<udemy.Models.Category>().HasData(
                new udemy.Models.Category { Id = 1, CategoryName = "abc", DisplayOrder = "1" },
                new udemy.Models.Category { Id = 2, CategoryName = "xyz", DisplayOrder = "2" }
            );
        }

    }
}
