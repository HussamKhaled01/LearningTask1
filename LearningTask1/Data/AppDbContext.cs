using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningTask1.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        
        
    }
}
