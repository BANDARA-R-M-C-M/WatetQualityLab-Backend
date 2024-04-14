using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_v1.Models;
using Project_v1.Models.Users;

namespace Project_v1.Data {
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration configuration) 
        : IdentityDbContext<SystemUser>(options) {

        private readonly IConfiguration _configuration = configuration;

        /*public DbSet<Admin> Admins { get; set; }
        public DbSet<Mlt> Mlts { get; set; }
        public DbSet<Phi> Phis { get; set; }
        public DbSet<Moh_supervisor> Moh_supervisors { get; set; }*/
        public DbSet<Lab> Labs { get; set; }
        public DbSet<PHIArea> PHIAreas { get; set; }
        public DbSet<MOHArea> MOHAreas { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(entity => {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            this.SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder) {
            builder.Entity<IdentityRole>().HasData(
                               new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                               new IdentityRole { Name = "Mlt", NormalizedName = "MLT" },
                               new IdentityRole { Name = "MohSupervisor", NormalizedName = "MOH_Supervisor" },
                               new IdentityRole { Name = "Phi", NormalizedName = "PHI" }
            );
        }
    }
}
