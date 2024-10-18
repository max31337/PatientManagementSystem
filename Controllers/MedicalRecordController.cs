using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class MedicalRecordController : Controller
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var records = _context.MedicalRecords.ToList();
        return View(records);
    }
}
