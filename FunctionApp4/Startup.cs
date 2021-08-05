using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(FunctionApp4.Startup))]
namespace FunctionApp4
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<AppDbContext>(
              options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        }

        public class FunctionAppContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));

                return new AppDbContext(optionsBuilder.Options);
            }
        }
    }
}
