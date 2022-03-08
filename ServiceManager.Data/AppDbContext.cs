
using Microsoft.EntityFrameworkCore;
using ServiceManager.Data.Entities;

namespace ServiceManager.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //private readonly IHttpContextAccessor _contextAccessor;
        //public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
        //{
        //    _contextAccessor = contextAccessor;
        //}

        public DbSet<Service> Services { get; set; }
    }
}