using Services.Contracts;

namespace Services.Implementation
{
    public class unitOfWork : IunitOfWork
    {
        public IVendorRepository vendor { get; private set; }
        public ISchedulingRepository scheduling { get; private set; }
        public IRecordRepository record { get; private set; }

        public unitOfWork(IVendorRepository Vendor, ISchedulingRepository scheduling, IRecordRepository record)
        {
            vendor = Vendor;
            this.scheduling = scheduling;
            this.record = record;
        }
    }
}
