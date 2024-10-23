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
        // Configure Payment entity
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)"); 
        });

        modelBuilder.Entity<User>()
            .ToTable("Users");

        base.OnModelCreating(modelBuilder);
    }
}
