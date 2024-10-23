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
    public virtual Patient Patient { get; set; }
}
