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

    //These are for charts
    public IActionResult Dashboard()
    {
        var genderLabels = new Dictionary<Gender, string>
    {
        { Gender.Male, "Male" },
        { Gender.Female, "Female" },
        { Gender.Other, "Other" }
    };

        var genderCount = _context.Patients
            .GroupBy(p => p.Gender)
            .ToDictionary(
                g => genderLabels[g.Key], 
                g => g.Count()
            );

        var ageDistribution = _context.Patients
            .Select(p => p.DateOfBirth)
            .ToList();

        var ageGroups = new Dictionary<string, int>();
        foreach (var dob in ageDistribution)
        {
            var age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now < dob.AddYears(age)) age--;
            if (age < 18) age = 18;
            var ageGroup = $"{age / 10 * 10}s"; 
            if (!ageGroups.ContainsKey(ageGroup))
            {
                ageGroups[ageGroup] = 0;
            }
            ageGroups[ageGroup]++;
        }

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
