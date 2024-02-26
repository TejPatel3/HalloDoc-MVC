namespace Services.ViewModels
{
    public class ViewCaseViewModel
    {
        public String ConfirmationNumber { get; }
        public String PatientNotes { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateOnly DOB { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }

        public String Region { get; set; }

        public String Address { get; set; }
        public String Room { get; set; }
    }
}
