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


    public async Task<IActionResult> Record (int patientId)
    {
        // Retrieve the patient's information
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

}
