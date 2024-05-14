using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_v1.Models;
using Project_v1.Models.Users;

namespace Project_v1.Data {
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration configuration) 
        : IdentityDbContext<SystemUser>(options) {

        private readonly IConfiguration _configuration = configuration;

        public DbSet<Lab> Labs { get; set; }
        public DbSet<PHIArea> PHIAreas { get; set; }
        public DbSet<MOHArea> MOHAreas { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<GeneralInventory> GeneralInventory { get; set; }
        public DbSet<GeneralCategory> GeneralCategory { get; set; }
        public DbSet<SurgicalInventory> SurgicalInventory { get; set; }
        public DbSet<SurgicalCategory> SurgicalCategory { get; set; }
        public DbSet<IssuedItem> IssuedItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            this.SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder) {
            builder.Entity<IdentityRole>().HasData(
                               new IdentityRole { Id = "R1", Name = "Admin", NormalizedName = "ADMIN" },
                               new IdentityRole { Id = "R2", Name = "Mlt", NormalizedName = "MLT" },
                               new IdentityRole { Id = "R3", Name = "MohSupervisor", NormalizedName = "MOH_Supervisor" },
                               new IdentityRole { Id = "R4", Name = "Phi", NormalizedName = "PHI" }
            );
        }
    }
}
