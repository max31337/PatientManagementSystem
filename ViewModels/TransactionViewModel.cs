using PatientManagementSystem.ViewModels;

namespace PatientManagementSystem.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int PatientId { get; set; }
        public string PatientID => $"PAT{PatientId:D3}";
        public required string Fullname { get; set; }
        public DateTime VisitDate { get; set; }
        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
    }
}