using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers.Invoicing
{
    public class InvoicingController : Controller
    {
        public IActionResult Invoicing()
        {
            return View();
        }
    }
}
