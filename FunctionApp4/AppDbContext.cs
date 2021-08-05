using Microsoft.EntityFrameworkCore;

namespace FunctionApp4
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<PC> PCs { get; set; }
    }
}
