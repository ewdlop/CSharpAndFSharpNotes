using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using Xunit;

namespace XUnitTestProject;

public partial class Mock
{
    private readonly Mock<IDbContextFactory<AppDbContext>> _mockDbFactory;
    public Mock()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _mockDbFactory = new Mock<IDbContextFactory<AppDbContext>>();

        //Seed 
        using var context = new AppDbContext(options);
        context.Employees.Add(new Employee { Id = 1, Name = "John", Designation = "Manager" });
        context.Employees.Add(new Employee { Id = 2, Name = "Smith", Designation = "Developer" });
        context.Employees.Add(new Employee { Id = 3, Name = "David", Designation = "Tester" });
        context.SaveChanges();

        _mockDbFactory.Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppDbContext(options));
    }

    [Theory]
    [InlineData(1, "John", "Manager")]
    public async void Test_GetEmployeeById(int id, string name, string desgination)
    {
        var service = new EmployeeService(_mockDbFactory.Object);
        Employee result = await service.GetEmployeeById(1);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(desgination, result.Designation);
    }


    [Fact]
    public void Teset_AddEmployee(Employee employee)
    {
        using var context = _mockDbFactory.Object.CreateDbContextAsync(CancellationToken.None).Result;
        context.Employees.Add(employee);
        context.SaveChanges();
    }
}