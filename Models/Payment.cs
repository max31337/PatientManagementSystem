using PatientManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsPaid { get; set; }

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }

}
