using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace XUnitTestProject;

public class EmployeeService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    public EmployeeService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        return await context.Employees.FindAsync(id);
    }

    public async Task AddEmployee(Employee employee)
    {
        using var context = await _dbFactory.CreateDbContextAsync();
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
    }
}