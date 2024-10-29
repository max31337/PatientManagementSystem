namespace PatientManagementSystem.ViewModels
{
    public class MedicalRecordViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string RecordDetails { get; set; }
        public string Allergies { get; set; }
        public string ReasonForVisit { get; set; }
        public string BloodTestResults { get; set; }
        public string PhysicalExamResults { get; set; }
        public string XRayImagePath { get; set; }
    }
}

