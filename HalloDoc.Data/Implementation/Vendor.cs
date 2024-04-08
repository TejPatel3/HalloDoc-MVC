using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class Vendor : IVendor
    {
        private readonly ApplicationDbContext _context;
        public Vendor()
        {
            _context = new ApplicationDbContext();
        }
        public List<Region> getRegionList()
        {
            var regions = _context.Regions.ToList();
            return regions;
        }
        public VendorViewModel getVendorData()
        {
            VendorViewModel model = new VendorViewModel();
            model.healthProfessionallist = _context.HealthProfessionals.ToList();
            model.healthProfessionalTypelist = _context.HealthProfessionalTypes.ToList();
            model.regionlist = _context.Regions.ToList();
            return model;
        }
    }
}
