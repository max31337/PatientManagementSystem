using PatientManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class MedicalRecordViewModel
    {
        public int PatientId { get; set; }
        public required string FullName{ get; set; }
        public string? RecordDetails { get; set; }
        public string? MedicalHistory { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Notes { get; set; }
        [Required]
        public required string Doctor { get; set; }
        [Required]
        public required string Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        [Required]
        public virtual required Patient Patient { get; set; }
        public virtual ICollection<LabRecord> LabRecords { get; set; } = new List<LabRecord>();
    }
}
