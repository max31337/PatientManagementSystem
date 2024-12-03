namespace PatientManagementSystem.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string? ProcedureName { get; set; }
        public decimal? ProcedurePrice { get; set; }
        public string? ProcedureDescription { get; set; }
        public string? ProcedureNotes { get; set; }
    }

}
