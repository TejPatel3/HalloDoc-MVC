using System.Diagnostics.CodeAnalysis;

namespace Services.ViewModels
{
    public class ViewCaseViewModel
    {
        public String? ConfirmationNumber { get; set; }

        public String PatientNotes { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public DateTime DOB { get; set; }

        public String PhoneNumber { get; set; }

        public String Email { get; set; }

        public String Region { get; set; }

        [MaybeNull]
        public String Address { get; set; }

        public String Room { get; set; }

        public int requestId { get; set; }

        public int status { get; set; }
    }
}
