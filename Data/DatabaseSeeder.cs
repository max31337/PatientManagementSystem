using PatientManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace PatientManagementSystem.Data
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedPatientsAsync();
            await SeedMedicalRecordsAsync();
            await SeedPaymentsAsync();
        }

        private async Task SeedPatientsAsync()
        {
            if (await _context.Patients.AnyAsync())
            {
                return; // Patients already seeded
            }

            var patients = new[]
            {
                new Patient
                {
                    FullName = "John Doe",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Address = "123 Main St"
                },
                new Patient
                {
                    FullName = "Jane Smith",
                    DateOfBirth = new DateTime(1990, 5, 5),
                    Address = "456 Elm St"
                },
                new Patient
                {
                    FullName = "Michael Johnson",
                    DateOfBirth = new DateTime(1985, 3, 15),
                    Address = "789 Maple Ave"
                }
            };

            await _context.Patients.AddRangeAsync(patients);
            await _context.SaveChangesAsync();
        }

        private async Task SeedMedicalRecordsAsync()
        {
            if (await _context.MedicalRecords.AnyAsync())
            {
                return; // MedicalRecords already seeded
            }

            var medicalRecords = new[]
            {

                new MedicalRecord
                {
                    PatientId = 3005,
                    RecordDetails = "Coughing won't stop",
                    Allergies = "None",
                    ReasonForVisit = "Follow up check up",
                    BloodTestResults = "Elevated",
                    PhysicalExamResults = "Normal",
                    XRayImagePath = "/images/xray1.jpg"
                },
                new MedicalRecord
                {
                    PatientId = 3006,
                    RecordDetails = "Routine check-up",
                    Allergies = "None",
                    ReasonForVisit = "Annual physical",
                    BloodTestResults = "Normal",
                    PhysicalExamResults = "Normal",
                    XRayImagePath = "/images/xray1.jpg"
                },
                new MedicalRecord
                {
                    PatientId = 3007,
                    RecordDetails = "Follow-up on allergies",
                    Allergies = "Peanuts",
                    ReasonForVisit = "Allergy consultation",
                    BloodTestResults = "Elevated",
                    PhysicalExamResults = "Normal",
                    XRayImagePath = "/images/xray1.jpg"
                }


            };

            await _context.MedicalRecords.AddRangeAsync(medicalRecords);
            await _context.SaveChangesAsync();
        }

        private async Task SeedPaymentsAsync()
        {
            if (await _context.Payments.AnyAsync())
            {
                return; // Payments already seeded
            }

            var payments = new[]
            {
                new Payment
                {
                    PatientId = 1,
                    Amount = 100.00m,
                    PaymentDate = DateTime.Now,
                    IsPaid = true
                },
                new Payment
                {
                    PatientId = 2,
                    Amount = 150.00m,
                    PaymentDate = DateTime.Now,
                    IsPaid = false
                },
                new Payment
                {
                    PatientId = 3,
                    Amount = 200.00m,
                    PaymentDate = DateTime.Now,
                    IsPaid = true
                }
            };

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();
        }
    }
}
