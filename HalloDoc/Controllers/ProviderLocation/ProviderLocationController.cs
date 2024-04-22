using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.ProviderLocation
{
    public class ProviderLocationController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public ProviderLocationController(IunitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public IActionResult ProviderLocation()
        {
            return View();
        }
        public string GetLocations()
        {
            var physicianlocation = _unitOfWork.tableData.GetPhysicianLocationList();
            var physician = _unitOfWork.tableData.GetPhysicianList();
            List<ProviderLocationViewModel> locations = new List<ProviderLocationViewModel>();
            foreach (var physicianLocation in physicianlocation)
            {
                locations.Add(new ProviderLocationViewModel
                {
                    Photo = physician.FirstOrDefault(x => x.PhysicianId == physicianLocation.PhysicianId).Photo,
                    Lat = physicianLocation.Latitude,
                    Long = physicianLocation.Longitude,
                    Physicianid = physicianLocation.PhysicianId,
                    Name = physician.FirstOrDefault(m => m.PhysicianId == physicianLocation.PhysicianId).FirstName + physician.FirstOrDefault(m => m.PhysicianId == physicianLocation.PhysicianId).LastName,
                });
            }
            return locations.ToJson();
        }
    }
}
