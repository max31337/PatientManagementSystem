using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;

namespace PatientManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<LabRecord> LabRecords { get; set; }
        public DbSet<AllergyHistory> AllergyHistories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Patient
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalRecords)
                .WithOne(mr => mr.Patient)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Payments)
                .WithOne(payment => payment.Patient)
                .HasForeignKey(payment => payment.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure MedicalRecord
            modelBuilder.Entity<MedicalRecord>()
                .HasMany(mr => mr.LabRecords)
                .WithOne(lr => lr.MedicalRecord)
                .HasForeignKey(lr => lr.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure AllergyHistory
            modelBuilder.Entity<AllergyHistory>()
                .HasOne(ah => ah.Patient)
                .WithMany()
                .HasForeignKey(ah => ah.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure LabRecord
            modelBuilder.Entity<LabRecord>()
                .Property(lr => lr.TestType)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<LabRecord>()
                .Property(lr => lr.Result)
                .IsRequired();

            modelBuilder.Entity<LabRecord>()
                .Property(lr => lr.Notes)
                .HasMaxLength(500);

            // Configure Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Patient)
                .WithMany(pt => pt.Payments)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .Property(p => p.InvoiceNumber)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasColumnType("decimal(18, 2)") // Precision 18, Scale 2
                .IsRequired();

            // Configure User
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();
        }
    }
}
