using HalloDoc.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IViewCaseRepo : IRepository<RequestClient>
    {
        public ViewCaseViewModel GetViewCaseData(int id);

        public void EditInfo(ViewCaseViewModel viewmodel);
    }
}
