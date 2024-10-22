using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;

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

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Patient patient)
    {
        if (ModelState.IsValid)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Patient patient)
    {
        if (ModelState.IsValid)
        {
            _context.Update(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        return View(patient);
    }
}