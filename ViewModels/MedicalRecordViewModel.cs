namespace PatientManagementSystem.ViewModels
{
    public class MedicalRecordViewModel
    {
        public int Id { get; set; } // Include the Id for editing purposes
        public int PatientId { get; set; }
        public string ReasonForVisit { get; set; }

        // Additional fields
        public string Allergies { get; set; }
        public string RecordDetails { get; set; }
        public string BloodTestResults { get; set; }
        public string PhysicalExamResults { get; set; }
        public string XRayImagePath { get; set; } // For displaying X-ray images
    }
}

