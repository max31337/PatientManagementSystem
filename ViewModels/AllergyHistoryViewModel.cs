using System;
using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class AllergyHistoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? ClinicalFindings { get; set; }

        public string? Medications { get; set; }

        public int PatientId { get; set; }  // Reference to the Adult Patient
    }
}