using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface IAdminLog
    {
        int AdminLogin(AspNetUser req);
    }
}