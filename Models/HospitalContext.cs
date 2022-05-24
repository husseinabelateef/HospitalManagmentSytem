using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagmentSytem.Models
{
    public class HospitalContext : IdentityDbContext<User>
    {
        public DbSet<appointment> appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public HospitalContext()
        {

        }
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            
        }
    }
}
