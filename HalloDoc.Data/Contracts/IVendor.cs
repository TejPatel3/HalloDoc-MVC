using HalloDoc.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IVendor
    {
        public List<Region> getRegionList();
        public VendorViewModel getVendorData();

    }
}
