using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using PatientManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MedicalRecordController : Controller
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordController(ApplicationDbContext context)
    {
        _context = context;
    }


    /*for testing
    private readonly RabbitMQService _rabbitMQService;
    public MedicalRecordController(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }


    public MedicalRecordController(ApplicationDbContext context)
    {
        _context = context;
    }
    */

    // List all medical records for a specific patient
    public async Task<IActionResult> Index(int patientId)
    {
        var records = await _context.MedicalRecords.ToListAsync();
        return View(records);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var patients = await _context.Patients
            .Select(p => new
            {
                p.Id,
                FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
            })
            .ToListAsync();

        ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(MedicalRecord model)
    {
        if (!ModelState.IsValid)
        {
            // Log validation errors for debugging
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            var patients = await _context.Patients
                .Select(p => new
                {
                    p.Id,
                    FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
                })
                .ToListAsync();

            ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName");
            return View(model);
        }

        _context.MedicalRecords.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }



    //Lists of records of a patient
    public async Task<IActionResult> Record(int patientId)
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

    // GET: MedicalRecord/Details/5
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var medicalRecord = await _context.MedicalRecords
            .Include(m => m.Patient)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicalRecord == null)
        {
            return NotFound();
        }

        var patients = await _context.Patients
            .Select(p => new
            {
                p.Id,
                FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
            })
            .ToListAsync();

        ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName", medicalRecord.PatientId);

        return View(medicalRecord);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, MedicalRecord model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            var patients = await _context.Patients
                .Select(p => new
                {
                    p.Id,
                    FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
                })
                .ToListAsync();

            ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName", model.PatientId);
            return View(model);
        }

        var existingRecord = await _context.MedicalRecords
            .Include(m => m.Patient)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingRecord == null)
        {
            return NotFound();
        }

        existingRecord.PatientId = model.PatientId;
        existingRecord.Doctor = model.Doctor;
        existingRecord.Department = model.Department;
        existingRecord.RecordDetails = model.RecordDetails;
        existingRecord.MedicalHistory = model.MedicalHistory;
        existingRecord.CurrentMedications = model.CurrentMedications;
        existingRecord.Notes = model.Notes;
        existingRecord.UpdatedAt = DateTime.UtcNow;

        _context.MedicalRecords.Update(existingRecord);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    // GET: MedicalRecord/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var medicalRecord = await _context.MedicalRecords
            .Include(mr => mr.Patient)
            .Include(mr => mr.LabRecords)
            .FirstOrDefaultAsync(mr => mr.Id == id);

        if (medicalRecord == null)
        {
            return NotFound();
        }

        return View(medicalRecord);
    }



    // GET: AddLabRecord (pre-populate the form)
    public IActionResult AddLabRecord(int medicalRecordId)
    {
        var model = new LabRecord
        {
            MedicalRecordId = medicalRecordId,
            // Set default values or leave empty if the form requires the user to input them
            TestType = "",
            Result = ""
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLabRecord(LabRecord labRecord, IFormFile? ImagePath)
    {
        // Log ModelState for debugging
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(labRecord);
        }

        if (ImagePath != null && ImagePath.Length > 0)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "labrecords");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, ImagePath.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImagePath.CopyToAsync(stream);
            }

            labRecord.ImagePath = "/labrecords/" + ImagePath.FileName;
        }

        labRecord.CreatedAt = DateTime.UtcNow;

        _context.LabRecords.Add(labRecord);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "MedicalRecord", new { id = labRecord.MedicalRecordId });
    }

    // GET: MedicalRecord/Labrecord/5
    public async Task<IActionResult> Labrecord(int id)
    {
        var labRecord = await _context.LabRecords
            .Include(mr => mr.MedicalRecord)
            .FirstOrDefaultAsync(mr => mr.Id == id);

        if (labRecord == null)
        {
            return NotFound();
        }

        return View(labRecord);
    }
}
