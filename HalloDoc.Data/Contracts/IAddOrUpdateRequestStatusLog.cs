namespace Services.Contracts
{
    public interface IAddOrUpdateRequestStatusLog
    {
        void AddOrUpdateRequestStatusLog(int requestid, string? cancelnote, int? AdminId = null, int? trnastophyid = null, int? PhysicianId = null);
    }
}
