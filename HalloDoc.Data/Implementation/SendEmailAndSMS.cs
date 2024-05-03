using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;
using System.Collections;
using System.Net;
using System.Net.Mail;

public class SendEmailAndSMS : ISendEmailAndSMS
{
    private readonly ApplicationDbContext _context;

    public SendEmailAndSMS(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SendSMS()
    {

        Smslog sms = new Smslog
        {
            MobileNumber = "7623971750",
            CreateDate = DateTime.Now,
            SentDate = DateTime.Now,
            IsSmssent = new BitArray(new[] { true }),
            SentTries = 1,
            Smstemplate = "main"
        };

        _context.Add(sms);
        _context.SaveChanges();

    }

    public async Task Sendemail(string email, string subject, string message)
    {
        try
        {
            var mail = "tatva.dotnet.yashsarvaiya@outlook.com";
            var password = "Yash@1234";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);


            EmailLog emaillog = new EmailLog();
            emaillog.SubjectName = subject;
            emaillog.EmailId = email;
            emaillog.CreateDate = DateTime.Now;
            emaillog.SentDate = DateTime.Now;
            emaillog.IsEmailSent = new BitArray(new[] { true });
            emaillog.EmailTemplate = "emailtemplate";

            _context.Add(emaillog);
            _context.SaveChanges();

            await client.SendMailAsync(mailMessage);



        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}