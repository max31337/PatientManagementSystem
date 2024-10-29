using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;
using System.IO;
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
    public async Task<IActionResult> Index()
    {
        // Retrieve all medical records along with their corresponding PatientId
        var records = await _context.MedicalRecords
            .Select(record => new MedicalRecordViewModel
            {
                Id = record.Id,
                PatientId = record.PatientId,
                ReasonForVisit = record.ReasonForVisit,
                // Add any additional fields you want to retrieve
                Allergies = record.Allergies,
                RecordDetails = record.RecordDetails,
                BloodTestResults = record.BloodTestResults,
                PhysicalExamResults = record.PhysicalExamResults,
                XRayImagePath = record.XRayImagePath
            }).ToListAsync();

        // Check if records are found
        if (records == null || !records.Any())
        {
            TempData["Message"] = "No records found.";
        }

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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var medicalRecord = await _context.MedicalRecords
            .Include(m => m.Patient) // Include related Patient information if needed
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicalRecord == null)
        {
            return NotFound();
        }

        var model = new MedicalRecordViewModel
        {
            Id = medicalRecord.Id,
            PatientId = medicalRecord.PatientId,
            RecordDetails = medicalRecord.RecordDetails,
            // Add any additional properties needed for editing
        };

        return View(model); // Return the edit view with the model
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MedicalRecordViewModel model, IFormFile xRayImage)
    {
        if (ModelState.IsValid)
        {
            // Find the existing medical record
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            // Update medical record details
            medicalRecord.RecordDetails = model.RecordDetails;

            // Handle the uploaded X-Ray image
            if (xRayImage != null && xRayImage.Length > 0)
            {
                // Optional: You might want to delete the old file if you're replacing it
                var oldFilePath = Path.Combine("wwwroot", medicalRecord.XRayImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                var filePath = Path.Combine("wwwroot/images/xrays", xRayImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await xRayImage.CopyToAsync(stream);
                }

                // Save the new file path to the medical record
                medicalRecord.XRayImagePath = $"/images/xrays/{xRayImage.FileName}";
            }

            _context.Update(medicalRecord);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { patientId = medicalRecord.PatientId });
        }

        // If we got this far, something failed; redisplay form.
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var record = await _context.MedicalRecords
            .Include(m => m.Patient) // Include the related Patient information
            .FirstOrDefaultAsync(m => m.Id == id);

        if (record == null)
        {
            return NotFound();
        }

        return View(record); // Return the details view with the record
    }
}
