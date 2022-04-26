
using Microsoft.EntityFrameworkCore;
using ServiceManager.Data.Entities;

namespace ServiceManager.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Service>().HasKey(m => m.Srn);
            //builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            //// shadow properties
            //builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            //builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }
        //private readonly IHttpContextAccessor _contextAccessor;
        //public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
        //{
        //    _contextAccessor = contextAccessor;
        //}

        public DbSet<Service> Services { get; set; }
    }
}