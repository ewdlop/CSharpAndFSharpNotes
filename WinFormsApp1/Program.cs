using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new HostBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 //services.AddDbContext<EmployeeContext>(options =>
                 //{
                 //    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;");
                 //});

                 services.AddDbContextPool<EmployeeContext>(
                    options => options.UseSqlServer(""));
             });

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var logg = services.GetRequiredService<ILogger<Form1>>();
                    var employeeContext = services.GetRequiredService<EmployeeContext>();

                    ApplicationConfiguration.Initialize();
                    Application.Run(new Form1(employeeContext));

                    Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured");
                }
            }

        }
    }
}