using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers.Provider
{
    public class ProviderController : Controller
    {
        public IActionResult Provider()
        {
            return View();
        }
    }
}
