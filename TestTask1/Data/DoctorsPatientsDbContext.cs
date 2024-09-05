using Microsoft.EntityFrameworkCore;
using TestTask1.Models;

namespace TestTask1.Data
{
    public class DoctorsPatientsDbContext : DbContext
    {
        public DoctorsPatientsDbContext(DbContextOptions<DoctorsPatientsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.District)
                .WithMany()
                .HasForeignKey(p => p.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Office)
                .WithMany()
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.District)
                .WithMany()
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
