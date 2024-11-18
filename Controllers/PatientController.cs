using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;
using PatientManagementSystem.Data;

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
                patientsQuery = patientsQuery.Where(p =>
                    p.FirstName.Contains(searchTerm) ||
                    (p.MiddleName != null && p.MiddleName.Contains(searchTerm)) ||
                    p.LastName.Contains(searchTerm));
            }
        }

        // Sorting
        switch (sortBy)
        {
            case "Name":
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.FirstName).ThenBy(p => p.LastName) :
                                patientsQuery.OrderByDescending(p => p.FirstName).ThenByDescending(p => p.LastName);
                break;
            case "DateOfBirth":
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.DateOfBirth) :
                                patientsQuery.OrderByDescending(p => p.DateOfBirth);
                break;
            case "Age":
                patientsQuery = sortOrder == "asc" ?
                                patientsQuery.OrderBy(p => p.Age) :
                                patientsQuery.OrderByDescending(p => p.Age);
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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientViewModel model)
    {
        if (ModelState.IsValid)
        {
            var patient = new Patient
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Age = model.Age,
                Gender = model.Gender,
                Address = model.Address,
                ContactNumber = model.ContactNumber,
                Occupation = model.Occupation,
                MaritalStatus = model.MaritalStatus,
                SpouseName = model.SpouseName,
                Employer = model.Employer,
                Weight = model.Weight,
                Height = model.Height,
                BloodPressure = model.BloodPressure,
                PastMedicalHistory = model.PastMedicalHistory,
                FamilyHistory = model.FamilyHistory,
                EmergencyPerson = model.EmergencyPerson,
                EmergencyRelationship = model.EmergencyRelationship,
                EmergencyContact = model.EmergencyContact,
                InsuranceDetails = model.InsuranceDetails,
                PrimaryCareProvider = model.PrimaryCareProvider,
                ParentGuardianName = model.ParentGuardianName,
                ParentGuardianContact = model.ParentGuardianContact,
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id, PatientViewModel model)
    {
        if (id != model.Id) // Ensure the ID matches the model's ID
        {
            return NotFound();
        }

        // Handle GET request (initial page load)
        if (Request.Method == HttpMethods.Get)
        {
            // Find the patient entity in the database by ID
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var viewModel = new PatientViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                MiddleName = patient.MiddleName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Age = patient.Age,
                Gender = patient.Gender,
                Address = patient.Address,
                ContactNumber = patient.ContactNumber,
                Occupation = patient.Occupation,
                Employer = patient.Employer,
                MaritalStatus = patient.MaritalStatus, 
                SpouseName = patient.SpouseName,
                Weight = patient.Weight,
                Height = patient.Height,
                BloodPressure = patient.BloodPressure,
                PastMedicalHistory = patient.PastMedicalHistory,
                FamilyHistory = patient.FamilyHistory,
                EmergencyPerson = patient.EmergencyPerson,
                EmergencyRelationship = patient.EmergencyRelationship,
                EmergencyContact = patient.EmergencyContact,
                InsuranceDetails = patient.InsuranceDetails,
                PrimaryCareProvider = patient.PrimaryCareProvider,
                ParentGuardianName = patient.ParentGuardianName,
                ParentGuardianContact = patient.ParentGuardianContact
            };

            return View(viewModel);
        }

        // Handle POST request (form submission)
        if (ModelState.IsValid)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            patient.FirstName = model.FirstName;
            patient.MiddleName = model.MiddleName;
            patient.LastName = model.LastName;
            patient.DateOfBirth = model.DateOfBirth;
            patient.Age = model.Age;
            patient.Gender = model.Gender;
            patient.Address = model.Address;
            patient.ContactNumber = model.ContactNumber;
            patient.Occupation = model.Occupation;
            patient.Employer = model.Employer;
            patient.MaritalStatus = model.MaritalStatus;
            patient.SpouseName = model.SpouseName;
            patient.Weight = model.Weight;
            patient.Height = model.Height;
            patient.BloodPressure = model.BloodPressure;
            patient.PastMedicalHistory = model.PastMedicalHistory;
            patient.FamilyHistory = model.FamilyHistory;
            patient.EmergencyPerson = model.EmergencyPerson;
            patient.EmergencyRelationship = model.EmergencyRelationship;
            patient.EmergencyContact = model.EmergencyContact;
            patient.InsuranceDetails = model.InsuranceDetails;
            patient.PrimaryCareProvider = model.PrimaryCareProvider;
            patient.ParentGuardianName = model.ParentGuardianName;
            patient.ParentGuardianContact = model.ParentGuardianContact;

            _context.Update(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(model);
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
}
