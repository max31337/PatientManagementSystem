using PatientManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MedicalRecord
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    public string RecordDetails { get; set; }

    // Additional fields
    public string Allergies { get; set; }
    public string ReasonForVisit { get; set; }

    public string BloodTestResults { get; set; }
    public string PhysicalExamResults { get; set; }

    // Path to X-ray images
    public string XRayImagePath { get; set; }

    public virtual Patient Patient { get; set; }
}
