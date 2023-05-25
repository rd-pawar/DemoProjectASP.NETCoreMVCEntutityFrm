using DemoProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
