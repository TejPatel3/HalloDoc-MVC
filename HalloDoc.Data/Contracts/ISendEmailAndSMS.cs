namespace Services.Contracts
{
    public interface ISendEmailAndSMS
    {
        public Task Sendemail(string email, string subject, string message);
        public void SendSMS();

    }
}
