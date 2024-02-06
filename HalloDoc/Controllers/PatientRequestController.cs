using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class PatientRequestController : Controller
    {
       public IActionResult CreateRequest()
        {
            return View();
        }

        public IActionResult Patient()
        {
            return View();
        }
        public IActionResult Business()
        {
            return View();
        }
        public IActionResult Concierge()
        {
            return View();
        }

        public IActionResult FamilyFriend()
        {
            return View();
        }



    }



}