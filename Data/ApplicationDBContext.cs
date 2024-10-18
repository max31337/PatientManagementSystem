using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<User> Users { get; set; } // I tried using Identitty but I think it is overkil, will be using it when need arise

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
