using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.ViewModels;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View(new ContactViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactViewModel model)
    {
        if (ModelState.IsValid)
        {
            //They think they really read this shit lmao
            TempData["SuccessMessage"] = "Thank you for contacting us! We will get back to you soon.";
            return RedirectToAction("Index");
        }

        return View(model); 
    }
}
