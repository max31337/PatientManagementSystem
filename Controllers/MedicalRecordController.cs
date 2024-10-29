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

    // List all medical records for a specific patient, This is unused I'll get to this sometime
    public async Task<IActionResult> Index(int patientId)
    {
        var records = _context.MedicalRecords.ToList(); 
        return View(records);
    }

    public IActionResult Create(int patientId)
    {
        ViewBag.PatientId = patientId; 
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalRecord(int patientId, string recordDetails)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            return Json(new { success = false, errors = "Patient not found." }); 
        }

        var newRecord = new MedicalRecord
        {
            PatientId = patientId,
            RecordDetails = recordDetails
        };

        _context.MedicalRecords.Add(newRecord);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
}
