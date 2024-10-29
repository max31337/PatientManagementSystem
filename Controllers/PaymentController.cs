using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;
using System.Linq;
using System.Threading.Tasks;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaymentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var payments = _context.Payments
            .Select(p => new PaymentViewModel
            {
                PatientId = p.PatientId,
                FullName = _context.Patients
                    .Where(pa => pa.Id == p.PatientId)
                    .Select(pa => pa.FullName)
            .FirstOrDefault() ?? string.Empty, 
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                IsPaid = p.IsPaid
            })
            .ToList();

        return View(payments);
    }

    public IActionResult Create(int patientId)
    {
        ViewBag.PatientId = patientId; 
        return PartialView(); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePayment(int patientId, PaymentViewModel paymentViewModel)
    {
        if (ModelState.IsValid)
        {
            var payment = new Payment
            {
                PatientId = patientId,
                Amount = paymentViewModel.Amount,
                PaymentDate = paymentViewModel.PaymentDate,
                IsPaid = paymentViewModel.IsPaid 
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray() });
    }

}
