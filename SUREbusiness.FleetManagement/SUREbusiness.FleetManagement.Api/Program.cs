using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SUREbusiness.FleetManagement.DAL;

namespace SUREbusiness.FleetManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .ConfigureDefaultDataSet()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
