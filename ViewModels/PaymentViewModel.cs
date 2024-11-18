using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels 
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }
        public string? FullName { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public required string ServicesAvailed { get; set; }

        [Required]
        [MaxLength(50)]
        public required string InvoiceNumber { get; set; }

        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; }
    }

}
