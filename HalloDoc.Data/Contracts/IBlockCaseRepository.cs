namespace Services.Contracts
{
    public interface IBlockCaseRepository
    {
        void BlockPatient(int requestid, String blocknote);
    }
}