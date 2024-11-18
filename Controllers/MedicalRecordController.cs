using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using PatientManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

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
        var records = await _context.MedicalRecords.ToListAsync(); 
        return View(records);
    }

    //Lists of records of a patient
    public async Task<IActionResult> Record (int patientId)
    {
        var patient = await _context.Patients
            .Where(p => p.Id == patientId)
            .FirstOrDefaultAsync();

        ViewData["FullName"] = patient!.FullName;

        var medicalRecords = await _context.MedicalRecords
            .Where(m => m.PatientId == patientId)
            .ToListAsync();

        if (medicalRecords == null || !medicalRecords.Any())
        {
            ViewData["NoMedicalRecords"] = "No medical records found for this patient.";
        }

        return View(medicalRecords);
    }

    // GET: MedicalRecord/Index
    public async Task<IActionResult> Index()
    {
        var medicalRecords = await _context.MedicalRecords
                                           .Include(mr => mr.Patient)
                                           .ToListAsync();
        return View(medicalRecords);
    }

    // GET: MedicalRecord/Details/5
    public async Task<IActionResult> Details(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var medicalRecord = await _context.MedicalRecords
                                          .Include(mr => mr.Patient)
                                          .Include(mr => mr.LabRecords) // I'll add this later
                                          .FirstOrDefaultAsync(mr => mr.Id == id);

        if (medicalRecord == null)
        {
            return NotFound();
        }

        return View(medicalRecord);
    }

}
