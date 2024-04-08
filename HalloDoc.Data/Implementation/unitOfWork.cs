using Services.Contracts;

namespace Services.Implementation
{
    public class unitOfWork : IunitOfWork
    {
        public IVendor vendor { get; private set; }

        public unitOfWork(IVendor Vendor)
        {
            vendor = Vendor;
        }
    }
}
