using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface IRecordRepository
    {
        public List<User> GetUserList(string firstname, string lastname, string email, int phone);

    }
}
