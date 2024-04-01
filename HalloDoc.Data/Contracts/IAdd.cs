using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdd
    {
        public void AddAdmin(UserAllDataViewModel obj, int adminid);
    }
}
