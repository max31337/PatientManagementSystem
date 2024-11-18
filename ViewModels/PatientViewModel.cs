using PatientManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string PatientID => $"PAT{Id:D3}";

        [Required(ErrorMessage = "First Name is required.")]
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public required string LastName { get; set; }

        //Computed property for Fullname
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? Occupation { get; set; }
        public string? Employer { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string? SpouseName { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "Please enter a valid weight")]
        public int? Weight { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage = "Please enter a valid height")]
        public int? Height { get; set; }
        [Required]
        [RegularExpression(@"^\d{2,3}/\d{2,3}$", ErrorMessage = "Please enter a valid blood pressure (e.g., 120/80)")]
        public string? BloodPressure { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public required string Address { get; set; }

        public string? PastMedicalHistory { get; set; }
        public string? FamilyHistory { get; set; }

        public string? EmergencyPerson { get; set; }
        public string? EmergencyRelationship{ get; set; }
        public string? EmergencyContact { get; set; }

        //Insurance details if they have 
        public string? InsuranceDetails { get; set; }
        public string? PrimaryCareProvider { get; set; }

        //If the patient is a minor (under 18 and not in Pediatric)
        public string? ParentGuardianName { get; set; }
        public string? ParentGuardianContact { get; set; }

        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public List<AllergyHistoryViewModel> AllergyHistories { get; set; } = new List<AllergyHistoryViewModel>();
        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
    }

}
