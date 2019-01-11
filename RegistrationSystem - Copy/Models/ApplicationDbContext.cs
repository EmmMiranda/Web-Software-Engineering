using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace RegistrationSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<System> Systems { get; set; }
        public DbSet<Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<System>().ToTable("System");
            modelBuilder.Entity<Module>().HasKey(c => new { c.Mod_Code, c.Mod_Sys_Code });
            modelBuilder.Entity<Module>().ToTable("Module");
        }
            
    }
}
