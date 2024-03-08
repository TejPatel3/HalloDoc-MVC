using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAddOrUpdateRequestNotes
    {
        void AddOrUpdateRequestNotes(AdminRequestViewModel obj);
    }
}
