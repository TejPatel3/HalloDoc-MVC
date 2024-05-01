using DataModels.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IVendorRepository
    {
        public List<Region> getRegionList();
        public List<HealthProfessionalType> GetProfessionList();
        public VendorViewModel getVendorData();

        public VendorViewModel EditVendorData(int vendorid);
        public VendorViewModel getFilteredVendorData(int professionid, string search, int vendorid);

        public bool AddVendorAccount(VendorViewModel model);
        public int EditVendorDataSubmit(VendorViewModel formdata);
    }
}
