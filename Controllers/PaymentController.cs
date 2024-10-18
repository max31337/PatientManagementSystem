using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaymentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var payments = _context.Payments.ToList();
        return View(payments);
    }
}
