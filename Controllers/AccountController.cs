using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.ViewModels;
using System.Threading.Tasks;

//I'll add the function for checking Users from other subsystems later
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Validate user credentials against the database
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password); // Adjust as needed

            if (user != null)
            {
                // Set session variable
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if passwords match
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            // Create a new user
            var user = new User { Email = model.Email, Username = model.Username, Password = model.Password }; // Hash password before saving in production!
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set session variable
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Dashboard", "Home");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        // Clear the session
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}
