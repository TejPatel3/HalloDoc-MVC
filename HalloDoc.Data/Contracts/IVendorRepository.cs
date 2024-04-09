using HalloDoc.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IVendorRepository
    {
        public List<Region> getRegionList();
        public VendorViewModel getVendorData();

    }
}
