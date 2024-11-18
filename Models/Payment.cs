using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        public required string ServicesAvailed { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }  // Make PaymentDate required if needed

        public bool IsPaid { get; set; } 

        [Required]
        [MaxLength(50)]
        public required string InvoiceNumber { get; set; }

        // Navigation Property 
        public virtual Patient? Patient { get; set; }
    }
}
