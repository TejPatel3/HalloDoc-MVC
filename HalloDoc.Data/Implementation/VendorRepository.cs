using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;

namespace Services.Implementation
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext _context;
        public VendorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Region> getRegionList()
        {
            var regions = _context.Regions.ToList();
            return regions;
        }
        public VendorViewModel getVendorData()
        {
            VendorViewModel model = new VendorViewModel();
            model.healthProfessionallist = _context.HealthProfessionals.Where(h => h.IsDeleted == new BitArray(new[] { false })).ToList();
            model.healthProfessionalTypelist = _context.HealthProfessionalTypes.ToList();
            model.regionlist = _context.Regions.ToList();
            return model;
        }

        public VendorViewModel getFilteredVendorData(int professionid, string search, int vendorid)
        {


            VendorViewModel model = new VendorViewModel();

            if (professionid != 0 && search != null)
            {
                model.healthProfessionallist = _context.HealthProfessionals.Where(h => h.Profession == professionid && h.IsDeleted == new BitArray(new[] { false }) && h.VendorName.ToLower().Contains(search.ToLower())).ToList();

            }
            else if (professionid != 0)
            {
                model.healthProfessionallist = _context.HealthProfessionals.Where(h => h.Profession == professionid && h.IsDeleted == new BitArray(new[] { false })).ToList();

            }
            else if (search != null)
            {
                List<HealthProfessional> searchdata = _context.HealthProfessionals.Where(h => h.VendorName.ToLower().Contains(search.ToLower()) && h.IsDeleted == new BitArray(new[] { false })).ToList();
                model.healthProfessionallist = searchdata;
            }


            if (vendorid != 0)
            {
                var x = _context.HealthProfessionals.FirstOrDefault(h => h.VendorId == vendorid);
                x.IsDeleted = new BitArray(new[] { true });
                _context.HealthProfessionals.Update(x);
                _context.SaveChanges();

                model.healthProfessionallist = _context.HealthProfessionals.Where(h => h.IsDeleted == new BitArray(new[] { false })).ToList();

            }

            model.healthProfessionalTypelist = _context.HealthProfessionalTypes.ToList();
            return model;

        }

        public VendorViewModel EditVendorData(int vendorid)
        {

            if (vendorid != 0)
            {
                var rowdata = _context.HealthProfessionals.FirstOrDefault(h => h.VendorId == vendorid);//for healthprofessional row data 
                var typeprofession = _context.HealthProfessionalTypes.FirstOrDefault(h => h.HealthProfessionalId == rowdata.Profession);

                VendorViewModel model = new VendorViewModel
                {
                    businessName = rowdata.VendorName,
                    profession = typeprofession.HealthProfessionalId.ToString(),
                    fax = rowdata.FaxNumber,
                    email = rowdata.Email,
                    phone = rowdata.PhoneNumber,
                    businesscontact = rowdata.BusinessContact,
                    street = rowdata.Address,
                    city = rowdata.City,
                    state = rowdata.State,
                    Zip = rowdata.Zip
                };

                model.healthProfessionalTypelist = _context.HealthProfessionalTypes.ToList();

                return model;

            }


            VendorViewModel mode = new VendorViewModel();
            if (vendorid == 0)
            {
                mode.healthProfessionalTypelist = _context.HealthProfessionalTypes.ToList();

                return mode;

            }
            return mode;
        }

        public bool AddVendorAccount(VendorViewModel model)
        {
            bool check = false;
            if (model != null)
            {
                BitArray bitset = new BitArray(new[] { false });
                HealthProfessional healthProfessional = new HealthProfessional
                {
                    VendorName = model.businessName,
                    Profession = int.Parse(model.profession),
                    FaxNumber = model.fax,
                    Address = model.street,
                    City = model.city,
                    State = model.state,
                    Zip = model.Zip,
                    RegionId = 2,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = model.phone,
                    Email = model.email,
                    BusinessContact = model.businesscontact,
                    IsDeleted = bitset,
                };
                _context.Add(healthProfessional);
                _context.SaveChanges();
                check = true;
            }
            return check;
        }
        public List<HealthProfessionalType> GetProfessionList()
        {
            List<HealthProfessionalType> model = _context.HealthProfessionalTypes.ToList();
            return model;
        }

        public int EditVendorDataSubmit(VendorViewModel formdata)        {            var rowdata = _context.HealthProfessionals.FirstOrDefault(h => h.VendorId == formdata.vendorid);//for healthprofessional row data 

            rowdata.VendorName = formdata.businessName;            rowdata.FaxNumber = formdata.fax;            rowdata.PhoneNumber = formdata.phone;            rowdata.Email = formdata.email;            rowdata.BusinessContact = formdata.businesscontact;            rowdata.Address = formdata.street;            rowdata.City = formdata.city;            rowdata.State = formdata.state;            rowdata.Zip = formdata.Zip;            rowdata.ModifiedDate = DateTime.Now;            if (rowdata != null)            {                _context.HealthProfessionals.Update(rowdata);                _context.SaveChanges();                return 1;            }            return 0;        }
    }
}
