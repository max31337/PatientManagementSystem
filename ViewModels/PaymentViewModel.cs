using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class PaymentViewModel
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }  // Add FullName property
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
