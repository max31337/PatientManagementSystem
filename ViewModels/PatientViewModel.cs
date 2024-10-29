using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
    }

}
