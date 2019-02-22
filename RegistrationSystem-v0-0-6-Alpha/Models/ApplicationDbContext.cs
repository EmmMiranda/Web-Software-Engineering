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
        public DbSet<Version> Versions { get; set; }
        public DbSet<Updates> Updates { get; set; }
        public DbSet<Enhancement> Enhancements { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SystemRegistration> SystemRegistrations { get; set; }
        public DbSet<ModuleRegistration> ModuleRegistrations { get; set; }
        public DbSet<EnhancementRegistration> EnhancementRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<System>().ToTable("System");

            modelBuilder.Entity<Module>().HasKey(c => new { c.Mod_Code, c.Mod_Sys_Code });
            modelBuilder.Entity<Module>().ToTable("Module");

            modelBuilder.Entity<Version>().HasKey(c => new { c.Ver_Sys_Code, c.Ver_Code });
            modelBuilder.Entity<Version>().ToTable("Version");

            modelBuilder.Entity<Updates>().HasKey(c => new { c.Upd_Sys_Code, c.Upd_Ver_Code, c.Upd_Code});
            modelBuilder.Entity<Updates>().ToTable("Updates");

            modelBuilder.Entity<Enhancement>().HasKey(c => new { c.Enh_Sys_Code, c.Enh_Mod_Code, c.Enh_Code });
            modelBuilder.Entity<Enhancement>().ToTable("Enhancement");

            modelBuilder.Entity<Customer>().ToTable("Customer");

            modelBuilder.Entity<SystemRegistration>().HasKey(c => new { c.Syr_Cst_Code,
                                                                        c.Syr_Sys_Code, c.Syr_Ver_Code });
            modelBuilder.Entity<SystemRegistration>().ToTable("SystemRegistration");

            modelBuilder.Entity<ModuleRegistration>().HasKey(c => new { c.Mor_Cst_Code, c.Mor_Sys_Code,
                                                                        c.Mor_Ver_Code, c.Mor_Mod_Code });
            modelBuilder.Entity<ModuleRegistration>().ToTable("ModuleRegistration");

            modelBuilder.Entity<EnhancementRegistration>().HasKey(c => new { c.Enr_Cst_Code, c.Enr_Sys_Code,
                                                                             c.Enr_Ver_Code, c.Enr_Mod_Code,
                                                                             c.Enr_Enh_Code });
            modelBuilder.Entity<EnhancementRegistration>().ToTable("EnhancementRegistration");
        }
            
    }
}
