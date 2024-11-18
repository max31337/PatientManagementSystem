using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;
using PatientManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                Id = p.Id,
                PatientId = p.PatientId,
                FullName = _context.Patients
                    .Where(pa => pa.Id == p.PatientId)
                    .Select(pa => pa.FullName)
                    .FirstOrDefault() ?? string.Empty,
                AmountPaid = p.AmountPaid,
                InvoiceNumber = p.InvoiceNumber,
                PaymentDate = p.PaymentDate,
                IsPaid = p.IsPaid,
                ServicesAvailed = p.ServicesAvailed
            })
            .ToList();

        return View(payments);
    }

    public async Task<IActionResult> Transactions(int patientId)
    {
        // Retrieve the patient's information
        var patient = await _context.Patients
            .Where(p => p.Id == patientId)
            .FirstOrDefaultAsync();

        ViewData["FullName"] = patient!.FullName;

        if (patient == null)
        {
            // Handle the case where the patient is not found
            return NotFound();
        }

        var payments = await _context.Payments
            .Where(p => p.PatientId == patientId)
            .ToListAsync();

        if (payments == null || !payments.Any())
        {
            ViewData["NoPayments"] = "No payments found for this patient.";
        }

        return View(payments);
    }

    // GET: Payments/Add
    public async Task<IActionResult> Add()
    {
        // Fetch patients for the dropdown
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
    public async Task<IActionResult> Add(PaymentViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            // Log or inspect errors for debugging
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                Console.WriteLine("Validation Error: " + error);
            }

            // Repopulate dropdown if validation fails
            var patients = await _context.Patients
                .Select(p => new
                {
                    p.Id,
                    FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
                })
                .ToListAsync();

            ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName", viewModel.PatientId);
            return View(viewModel);
        }

        // Ensure the patient exists in the database
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == viewModel.PatientId);
        if (!patientExists)
        {
            ModelState.AddModelError(string.Empty, "Invalid patient selected.");

            // Repopulate dropdown for the invalid case
            var patients = await _context.Patients
                .Select(p => new
                {
                    p.Id,
                    FullName = $"{p.FirstName} {p.MiddleName} {p.LastName}"
                })
                .ToListAsync();

            ViewBag.PatientDropdown = new SelectList(patients, "Id", "FullName", viewModel.PatientId);
            return View(viewModel);
        }



        // Map the ViewModel to the Payment model
        var payment = new Payment
        {
            PatientId = viewModel.PatientId,
            ServicesAvailed = viewModel.ServicesAvailed,
            AmountPaid = viewModel.AmountPaid,
            InvoiceNumber = viewModel.InvoiceNumber,
            PaymentDate = viewModel.PaymentDate,
            IsPaid = viewModel.IsPaid,
        };

        var paymentStatusList = new List<SelectListItem>
        {
            new SelectListItem { Text = "Paid", Value = "true", Selected = payment.IsPaid },
            new SelectListItem { Text = "Pending", Value = "false", Selected = !payment.IsPaid }
        };


        // Add payment record to the database
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Payment successfully added.";
        return RedirectToAction("Index");
    }

    // GET: Payments/Update/{id}
    public async Task<IActionResult> Update(int id)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        // Fetch the associated patient for display
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == payment.PatientId);

        if (patient == null)
        {
            return NotFound();
        }

        // Create the ViewModel and pass it to the View
        var viewModel = new PaymentViewModel
        {
            Id = payment.Id,
            PatientId = payment.PatientId,
            FullName = $"{patient.FirstName} {patient.MiddleName} {patient.LastName}",  // Full name of the patient
            AmountPaid = payment.AmountPaid,
            InvoiceNumber = payment.InvoiceNumber,
            PaymentDate = payment.PaymentDate,
            IsPaid = payment.IsPaid,
            ServicesAvailed = payment.ServicesAvailed
        };

        // Repopulate the payment status dropdown
        var paymentStatusList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Paid", Value = "true", Selected = payment.IsPaid },
        new SelectListItem { Text = "Pending", Value = "false", Selected = !payment.IsPaid }
    };
        ViewBag.PaymentStatus = paymentStatusList;

        return View(viewModel);
    }

    // POST: Payments/Update/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, PaymentViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            // Repopulate the payment status dropdown if validation fails
            var paymentStatusList = new List<SelectListItem>
        {
            new SelectListItem { Text = "Paid", Value = "true", Selected = viewModel.IsPaid },
            new SelectListItem { Text = "Pending", Value = "false", Selected = !viewModel.IsPaid }
        };
            ViewBag.PaymentStatus = paymentStatusList;

            return View(viewModel);
        }

        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        // Update the payment record
        payment.AmountPaid = viewModel.AmountPaid;
        payment.InvoiceNumber = viewModel.InvoiceNumber;
        payment.PaymentDate = viewModel.PaymentDate;
        payment.IsPaid = viewModel.IsPaid;
        payment.ServicesAvailed = viewModel.ServicesAvailed;

        // Save the changes to the database
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Payment successfully updated.";
        return RedirectToAction("Index");
    }


}
