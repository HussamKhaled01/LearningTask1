using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningTask1.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<BusinessCard> BusinessCards => Set<BusinessCard>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BusinessCard>().HasData(
             new BusinessCard { Id = 1, Name = "John Doe", Gender = "Male", DOB = new DateOnly(1990, 1, 1), Email = "john@example.com", PhoneNumber = "1234567890", Address = "123 Main St", ImageUrl = null },
             new BusinessCard { Id = 2, Name = "Jane Smith", Gender = "Female", DOB = new DateOnly(1992, 5, 10), Email = "jane@example.com", PhoneNumber = "0987654321", Address = "456 Oak Ave", ImageUrl = null },
             new BusinessCard { Id = 3, Name = "Michael Brown", Gender = "Male", DOB = new DateOnly(1985, 3, 15), Email = "michael@example.com", PhoneNumber = "1112223333", Address = "789 Pine Rd", ImageUrl = null },
             new BusinessCard { Id = 4, Name = "Emily White", Gender = "Female", DOB = new DateOnly(1995, 7, 22), Email = "emily@example.com", PhoneNumber = "4445556666", Address = "321 Cedar Ln", ImageUrl = null },
             new BusinessCard { Id = 5, Name = "David Green", Gender = "Male", DOB = new DateOnly(1988, 11, 30), Email = "david@example.com", PhoneNumber = "7778889999", Address = "654 Maple St", ImageUrl = null },
             new BusinessCard { Id = 6, Name = "Sophia Blue", Gender = "Female", DOB = new DateOnly(1993, 2, 5), Email = "sophia@example.com", PhoneNumber = "2223334444", Address = "987 Birch Ave", ImageUrl = null }
         );
        }

    }
}
