using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;


public class PatientController : Controller
{
    private readonly ApplicationDbContext _context;

    public PatientController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var patients = _context.Patients.ToList();
        return View(patients);
    }

    public IActionResult Create()
    {
        var viewModel = new PatientViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Return the view with validation messages if the model is invalid
            return View(model);
        }

        // Map the ViewModel to the Patient model
        var patient = new Patient
        {
            FullName = model.FullName,
            DateOfBirth = model.DateOfBirth,
            Address = model.Address
        };

        // Save to the database
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // Assign the generated patient ID to the ViewModel for display
        model.Id = patient.Id;

        // Display success message and patient ID
        return View(model);
    }




    public async Task<IActionResult> Edit(int id)
    {
        if (Request.Method == HttpMethods.Post)
        {
            var model = new PatientViewModel();
            await TryUpdateModelAsync(model);

            if (ModelState.IsValid)
            {
                var patient = await _context.Patients.FindAsync(model.Id);
                if (patient == null) return NotFound();

                patient.FullName = model.FullName;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Address = model.Address;

                _context.Update(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        else
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var viewModel = new PatientViewModel
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
            };

            return View(viewModel); 
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Payments) 
            .Include(p => p.MedicalRecords) 
            .FirstOrDefaultAsync(p => p.Id == id); 

        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    public async Task<JsonResult> Search(string searchTerm, string sortBy, string sortOrder)
    {
        var patientsQuery = _context.Patients.AsQueryable();

        // Filter by search term (both name and Patient ID)
        if (!string.IsNullOrEmpty(searchTerm))
        {
            if (int.TryParse(searchTerm, out int patientId))
            {
                // Search by Patient ID
                patientsQuery = patientsQuery.Where(p => p.Id == patientId);
            }
            else
            {
                // Search by Full Name
                patientsQuery = patientsQuery.Where(p => p.FullName.Contains(searchTerm));
            }
        }

        // Sorting
        switch (sortBy)
        {
            case "Name":
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.FullName) :
                                patientsQuery.OrderByDescending(p => p.FullName);
                break;
            case "DateOfBirth":
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.DateOfBirth) :
                                patientsQuery.OrderByDescending(p => p.DateOfBirth);
                break;
            default:
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.Id) :
                                patientsQuery.OrderByDescending(p => p.Id);
                break;
        }

        var patients = await patientsQuery.ToListAsync();

        return Json(patients);
    }

}
