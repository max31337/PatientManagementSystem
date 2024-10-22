using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    // This action will show the login and register buttons
    public IActionResult Index()
    {
        ViewData["HideSidebar"] = true;
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

}
