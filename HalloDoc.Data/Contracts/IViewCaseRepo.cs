using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface IViewCaseRepo : IRepository<RequestClient>
    {
        public RequestClient GetViewCaseData(int id);
    }
}
