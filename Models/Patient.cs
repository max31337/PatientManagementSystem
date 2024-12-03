using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace PatientManagementSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }

        //Important Details
        [Required]
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required]
        public required DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public required string Address { get; set; }
        /*[Required]
        public required string Email { get; set; }
        */
        public string? ContactNumber { get; set; }

        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string? BloodPressure { get; set; }

        public string? Occupation { get; set; }
        public string? Employer { get; set; }

        public required MaritalStatus MaritalStatus { get; set; }
        public string? SpouseName { get; set; }


        //Emergency Contact Details
        public string? EmergencyPerson { get; set; }
        public string? EmergencyRelationship { get; set; }
        public string? EmergencyContact { get; set; }

        public string? PastMedicalHistory { get; set; }
        public string? FamilyHistory { get; set; }

        //If Patient has insurance
        public string? InsuranceDetails { get; set; }
        public string? PrimaryCareProvider { get; set; }

        //If the patient is a minor (under 18 and not in Pediatric)
        public string? ParentGuardianName { get; set; }
        public string? ParentGuardianContact { get; set; }


        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }

    }

    public enum MaritalStatus
    {
        Single,
        Married,
        Separated,
        Widowed
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

}