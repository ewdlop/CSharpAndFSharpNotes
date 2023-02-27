using Microsoft.EntityFrameworkCore;

namespace XUnitTestProject;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Employee> Employees { get; set; }
}