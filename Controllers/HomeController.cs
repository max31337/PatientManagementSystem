using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    // This action will show the login and register buttons
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

}
