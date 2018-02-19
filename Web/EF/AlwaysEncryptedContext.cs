using System.Data.Entity;
using Web.Models;

namespace Web.EF
{
    public class AlwaysEncryptedContext : DbContext
    {
        public AlwaysEncryptedContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(patient => patient.SSN).HasMaxLength(11).IsRequired();
            modelBuilder.Entity<Patient>().Property(patient => patient.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Patient>().Property(patient => patient.LastName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Patient>().Property(patient => patient.MiddleName).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Patient>().Property(patient => patient.StreetAddress).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Patient>().Property(patient => patient.City).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Patient>().Property(patient => patient.State).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Patient>().Property(patient => patient.BirthDate).IsRequired();

            modelBuilder.Entity<Patient>().HasMany(patient => patient.Visits).WithRequired(visit => visit.Patient)
                .HasForeignKey(visit => visit.PatientId);

            modelBuilder.Entity<Visit>().Property(visit => visit.Date).IsRequired();
            modelBuilder.Entity<Visit>().Property(visit => visit.Reason).IsMaxLength().IsOptional();
            modelBuilder.Entity<Visit>().Property(visit => visit.Treatment).IsMaxLength().IsOptional();
            modelBuilder.Entity<Visit>().Property(visit => visit.FollowUpDate).IsOptional();

            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Staff>().Property(staff => staff.FirstName).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Staff>().Property(staff => staff.LastName).HasMaxLength(50).IsOptional();
            modelBuilder.Entity<Staff>().Property(staff => staff.Role).HasMaxLength(50).IsOptional();

            modelBuilder.Entity<Staff>().HasMany(staff => staff.Patients).WithMany(patient => patient.AssignedStaff)
                .Map(c => c.MapLeftKey("StaffId").MapRightKey("PatientId").ToTable("StaffAssignments"));
        }
    }
}