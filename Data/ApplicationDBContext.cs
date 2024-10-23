using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .ToTable("Patients");

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2");

            entity.HasOne(p => p.Patient) 
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.Property(e => e.RecordDetails)
                .IsRequired(); 

            entity.HasOne(m => m.Patient) 
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<User>()
            .ToTable("Users");

        base.OnModelCreating(modelBuilder);
    }
}
