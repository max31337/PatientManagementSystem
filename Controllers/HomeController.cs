using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementSystem.Data;
using PatientManagementSystem.Models;
using System.Linq;

public class HomeController : Controller


{

    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }


    // This action will show the login and register buttons
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        // Fetch gender count
        var genderLabels = new Dictionary<Gender, string>
    {
        { Gender.Male, "Male" },
        { Gender.Female, "Female" },
        { Gender.Other, "Other" }
    };

        var genderCount = _context.Patients
            .GroupBy(p => p.Gender)
            .ToDictionary(
                g => genderLabels[g.Key], // Convert enum to string
                g => g.Count()
            );

        // Fetch age distribution
        var ageDistribution = _context.Patients
            .Select(p => p.DateOfBirth)
            .ToList();

        // Age groups by decade
        var ageGroups = new Dictionary<string, int>();
        foreach (var dob in ageDistribution)
        {
            var age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now < dob.AddYears(age)) age--;
            if (age < 18) age = 18;
            var ageGroup = $"{age / 10 * 10}s"; // Group by decades (e.g., 20s, 30s, etc.)
            if (!ageGroups.ContainsKey(ageGroup))
            {
                ageGroups[ageGroup] = 0;
            }
            ageGroups[ageGroup]++;
        }

        // Fetch age categories (Minors, Adults, Seniors)
        var ageCategories = new Dictionary<string, int>
    {
        { "Minors (<18)", _context.Patients.Count(p =>
            (DateTime.Now.Year - p.DateOfBirth.Year) -
            (DateTime.Now < p.DateOfBirth.AddYears(DateTime.Now.Year - p.DateOfBirth.Year) ? 1 : 0) < 18) },

        { "Adults (18-65)", _context.Patients.Count(p =>
            (DateTime.Now.Year - p.DateOfBirth.Year) -
            (DateTime.Now < p.DateOfBirth.AddYears(DateTime.Now.Year - p.DateOfBirth.Year) ? 1 : 0) >= 18 &&
            (DateTime.Now.Year - p.DateOfBirth.Year) -
            (DateTime.Now < p.DateOfBirth.AddYears(DateTime.Now.Year - p.DateOfBirth.Year) ? 1 : 0) <= 65) },

        { "Seniors (>65)", _context.Patients.Count(p =>
            (DateTime.Now.Year - p.DateOfBirth.Year) -
            (DateTime.Now < p.DateOfBirth.AddYears(DateTime.Now.Year - p.DateOfBirth.Year) ? 1 : 0) > 65) }
    };

        // Pass the data to the view using ViewBag
        ViewBag.GenderCount = genderCount;
        ViewBag.AgeGroups = ageGroups;
        ViewBag.AgeCategories = ageCategories;

        return View();
    }




    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View("Contact");
    }


}
