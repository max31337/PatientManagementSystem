using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<User> Users { get; set; } // Add your custom User entity here

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the User table if necessary
        modelBuilder.Entity<User>()
            .ToTable("Users");
    }
}
