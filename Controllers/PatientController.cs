using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Models;
using PatientManagementSystem.ViewModels;
using PatientManagementSystem.Data;
using System.Drawing.Printing;
using System.Net.Mail;
using System.Net;

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

        if (!string.IsNullOrEmpty(searchTerm))
        {
            if (int.TryParse(searchTerm, out int patientId))
            {
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
                //Email = model.Email,
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
        if (id != model.Id)
        {
            return NotFound();
        }

        // Handle GET request (initial page load)
        if (Request.Method == HttpMethods.Get)
        {
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
                //Email = patient.Email,
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
            //patient.Email = model.Email;
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

    /*

    [HttpPost]
    public IActionResult SendInvoiceEmail(int paymentId)
    {
        // Fetch payment and patient details from the database
        var payment = _context.Payments
            .Where(p => p.Id == paymentId)
            .Select(p => new PaymentViewModel
            {
                InvoiceNumber = p.InvoiceNumber,
                FullName = p.Patient.FullName,
                AmountPaid = p.AmountPaid,
                IsPaid = p.IsPaid,
                ServicesAvailed = p.ServicesAvailed,
                PaymentDate = p.PaymentDate,
                Email = p.Patient.Email // Get the patient's email
            })
            .FirstOrDefault();

        if (payment == null || string.IsNullOrEmpty(payment.Email))
        {
            return BadRequest("Payment not found or email is unavailable for the associated patient.");
        }

        // Generate the PDF for the invoice
        var pdfPath = GenerateInvoicePdf(payment);

        // Send the email with the invoice PDF attached
        var emailSent = SendEmailWithAttachment(
            toEmail: payment.Email,
            subject: "Your Invoice - Alpha Star Laboratory & Multi-Specialty Clinic",
            body: "Dear " + payment.FullName + ",\n\nPlease find attached the invoice for your recent payment. Contact us if you have any questions.\n\nBest regards,\nAlpha Star Laboratory & Multi-Specialty Clinic",
            attachmentPath: pdfPath
        );

        // Clean up the temporary file after sending the email
        if (System.IO.File.Exists(pdfPath))
        {
            System.IO.File.Delete(pdfPath);
        }

        return emailSent ? Ok("Invoice sent successfully to " + payment.Email) : StatusCode(500, "Failed to send email.");
    }

    private string GenerateInvoicePdf(PaymentViewModel model)
    {
        string pdfPath = Path.Combine(Path.GetTempPath(), $"Invoice_{model.InvoiceNumber}.pdf");

        using (var stream = new FileStream(pdfPath, FileMode.Create))
        {
            Document document = new Document(PageSize.A4);
            PdfWriter.GetInstance(document, stream);
            document.Open();

            // Add content to the PDF
            document.Add(new Paragraph("Alpha Star Laboratory & Multi-Specialty Clinic", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
            document.Add(new Paragraph($"Invoice Number: {model.InvoiceNumber}"));
            document.Add(new Paragraph($"Payment Date: {model.PaymentDate:yyyy-MM-dd HH:mm}"));
            document.Add(new Paragraph($"Patient Name: {model.FullName}"));
            document.Add(new Paragraph($"Amount Paid: ₱{model.AmountPaid:0.00}"));
            document.Add(new Paragraph($"Payment Status: {(model.IsPaid ? "Paid" : "Pending")}"));
            document.Add(new Paragraph($"Services Availed: {model.ServicesAvailed}"));

            document.Close();
        }

        return pdfPath;
    }

    private bool SendEmailWithAttachment(string toEmail, string subject, string body, string attachmentPath)
    {
        try
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("alphastar@gmail.com"); // Replace with your email address
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.Attachments.Add(new Attachment(attachmentPath));

                using (var smtp = new SmtpClient("smtp.example.com")) // Replace with your SMTP server
                {
                    smtp.Port = 587; // Use appropriate port (587 for TLS)
                    smtp.Credentials = new NetworkCredential("alphastar@gmail.com", "--their password--"); // Replace with your credentials
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
    */
}