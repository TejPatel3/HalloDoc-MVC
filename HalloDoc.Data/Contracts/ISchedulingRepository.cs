using Services.ViewModels;

namespace Services.Contracts
{
    public interface ISchedulingRepository
    {
        public SchiftsForReviewViewModel getShiftData(string currentPartial, string date, int regionid, int pagesize, int currentpage);
        public int totalpagecount(string currentPartial, string date, int regionid);

    }
}
