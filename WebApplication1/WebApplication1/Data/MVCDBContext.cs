using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class MVCDBContext : DbContext
    {
        public MVCDBContext(DbContextOptions<MVCDBContext> options) : base (options)
        {
        
        
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
