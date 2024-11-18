using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementSystem.Models
{
    public class LabRecord
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("MedicalRecord")]
        public int MedicalRecordId { get; set; } 

        [Required]
        [MaxLength(100)]
        public required string TestType { get; set; } 

        [Required]
        public required string Result { get; set; } 

        [MaxLength(500)]
        public string? Notes { get; set; } 

        public string? ImagePath { get; set; } // File path for test images (e.g., X-rays, ECGs)

        public DateTime TestDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        [Required]
        public virtual required MedicalRecord MedicalRecord { get; set; } 
    }
}
