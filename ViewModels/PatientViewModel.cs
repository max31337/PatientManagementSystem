using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string RecordDetails { get; set; }

        [Required]
        public string Address { get; set; }
        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
    }

}
