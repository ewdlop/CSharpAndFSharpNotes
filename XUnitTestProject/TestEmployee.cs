using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using Xunit;
namespace XUnitTestProject;

public partial class TestEmployee
{
    private readonly Mock<IDbContextFactory<AppDbContext>> _mockDbFactory;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<Computer> _mockComputer;
    private readonly Computer _mockComputerLinq;
    public TestEmployee()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _mockDbFactory = new Mock<IDbContextFactory<AppDbContext>>(MockBehavior.Strict);
        _mockEmployeeRepository = new Mock<IEmployeeRepository>(MockBehavior.Strict);
        //Seed 
        using var context = new AppDbContext(options);
        context.Employees.Add(new Employee { Id = 1, Name = "John", Designation = "Manager" });
        context.Employees.Add(new Employee { Id = 2, Name = "Smith", Designation = "Developer" });
        context.Employees.Add(new Employee { Id = 3, Name = "David", Designation = "Tester" });
        context.SaveChanges();

        _mockDbFactory.Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AppDbContext(options));

        //stubbing all the properties
        //call this first
        _mockEmployeeRepository.SetupAllProperties();
        
        //for method with out parameter scenario
        _mockEmployeeRepository.SetupSequence(x => x.TryGetEmployee(It.IsAny<int>(), out It.Ref<Employee>.IsAny))
            .Returns(true)
            .Throws(new NotImplementedException());

        //for property scenario
        _mockEmployeeRepository.SetupGet(x => x.Count).Returns(0);

        //for hierarchical property scenario
        _mockEmployeeRepository.Setup(x => x.BestEmployee.Name).Returns("John");

        _mockHttpClientFactory.DefaultValue = DefaultValue.Mock;

        //stubbing a property
        _mockEmployeeRepository.SetupProperty(x => x.BestEmployee).Raise(x => x.BestEmployeeChangd += null, EventArgs.Empty); ;

        List<Employee> bestDeveloperEmployees = new();

        _mockEmployeeRepository.SetupSet(x => x.BestEmployee =
        Moq.Capture.With(new CaptureMatch<Employee>(employee => bestDeveloperEmployees.Add(employee), 
            employee => employee.Designation == "Developer")));


        _mockComputer = new Mock<Computer>();
        //Add method has to be Virtual
        _mockComputer.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
        //Multiply method has to be Virtual
        _mockComputer.Protected().Setup<int>("Multiply", ItExpr.IsAny<int>(), ItExpr.IsAny<int>()).Returns(10);

        _mockComputerLinq = Mock.Of<Computer>(computer =>
            computer.Add(It.IsAny<int>(), It.IsAny<int>()) == 10);

        var calculator = Mock.Of<Calculator<int>>(calculator =>
            calculator.Add(It.IsAny<int>(), It.IsAny<int>()) == 10);
    }
    
    public static T CaptureIn<T>(IList<T> collection, Expression<Func<T, bool>> predicate)
    {
        var match = new CaptureMatch<T>(collection.Add, predicate);
        return Moq.Capture.With(match);
    }
    [Theory]
    [InlineData(1, "John", "Manager")]
    public async void Test_GetEmployeeById(int id, string name, string desgination)
    {
        var service = new EmployeeService(_mockDbFactory.Object, _mockEmployeeRepository.Object);
        Employee result = await service.GetEmployeeById(1);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(desgination, result.Designation);
        _mockDbFactory.Verify(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()), Times.Once, "CreateDbContextAsync should be called only once");
        _mockDbFactory.Verify(factory => factory.CreateDbContext(), Times.Never);
        _mockEmployeeRepository.VerifyGet(repo => repo.Count, Times.Never);
    }


    [Fact]
    public void Teset_AddEmployee()
    {
        Employee gary = new Employee { Id = 4, Name = "Gary", Designation = "Developer" };
        using var context = _mockDbFactory.Object.CreateDbContextAsync(CancellationToken.None).Result;
        context.Employees.Add(gary);
        context.SaveChanges();
        var service = new EmployeeService(_mockDbFactory.Object, _mockEmployeeRepository.Object);
        service.SetBestEmployee(gary);
        _mockEmployeeRepository.VerifySet(repo => repo.BestEmployee = gary, Times.Once);
    }
}