using Microsoft.AspNetCore.Mvc;
using PatientManagementSystem.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData["HideSidebar"] = true;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Find the user by username
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user != null && VerifyPassword(user.Password, model.Password))
            {
                // Set session variables
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserRole", user.Role.ToString());
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    // Method to verify the password
    public bool VerifyPassword(string hashedPassword, string password)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
            throw new FormatException("Invalid hash format.");

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        var hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return hash == hashedInput;
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewData["HideSidebar"] = true; 
        return View();
    }


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

            // Hash the password before saving
            var hashedPassword = HashPassword(model.Password);

            // Create a new user
            var user = new User
            {
                Email = model.Email,
                Username = model.Username,
                Password = hashedPassword,
                Role = model.UserRole // Assuming you added the role in the RegisterViewModel
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set session variables
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("Dashboard", "Home");
        }
        return View(model);
    }

    private string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return Convert.ToBase64String(salt) + "." + hashed;
    }


    [HttpPost]
    public IActionResult Logout()
    {
        // Clear the session
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
