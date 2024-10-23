using Microsoft.AspNetCore.Mvc;
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
        if (ModelState.IsValid)
        {
            var patient = new Patient
            {
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Set the Id back to the model
            model.Id = patient.Id;

            // Return the view with the updated model
            return View(model);

        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        // Check if the request is a POST (form submission) or a GET (initial page load)
        if (Request.Method == HttpMethods.Post)
        {
            // Model binding for form submission
            var model = new PatientViewModel();
            await TryUpdateModelAsync(model);

            if (ModelState.IsValid)
            {
                var patient = await _context.Patients.FindAsync(model.Id);
                if (patient == null) return NotFound();

                // Update the patient details
                patient.FullName = model.FullName;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Address = model.Address;

                _context.Update(patient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Return to the view with the current model if the validation fails
            return View(model);
        }
        else
        {
            // Handle GET request: load the existing patient details
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            // Populate the view model with patient data
            var viewModel = new PatientViewModel
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
            };

            return View(viewModel); // Return the view with the existing details
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }
}
