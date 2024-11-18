using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementSystem.Models
{
    public class AllergyHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? ClinicalFindings { get; set; }
        public string? Medications { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; } 
        [Required]
        public virtual required Patient Patient { get; set; }
    }
}