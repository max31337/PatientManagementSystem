using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;

public class MedicalRecordController : Controller
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordController(ApplicationDbContext context)
    {
        _context = context;
    }

    // List all medical records for a specific patient
    public async Task<IActionResult> Index(int patientId)
    {
        var records = _context.MedicalRecords.ToList(); // Adjust as needed
        return View(records);
    }

    // Render the form to create a new medical record for a patient
    public IActionResult Create(int patientId)
    {
        ViewBag.PatientId = patientId; // Pass patient ID to the view
        return View();
    }

    // Handle form submission to create a new medical record (AJAX)
    [HttpPost]
    public async Task<IActionResult> CreateMedicalRecord(int patientId, string recordDetails)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            return Json(new { success = false, errors = "Patient not found." }); // Return JSON error
        }

        var newRecord = new MedicalRecord
        {
            PatientId = patientId,
            RecordDetails = recordDetails
        };

        _context.MedicalRecords.Add(newRecord);
        await _context.SaveChangesAsync();

        // Return success response
        return Json(new { success = true });
    }
}
