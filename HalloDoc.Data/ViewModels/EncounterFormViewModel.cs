using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DataModels.AdminSideViewModels
{
    public class EncounterFormViewModel
    {
        public int RequestId { get; set; }
        public string role { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Lastname { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public DateTime? Dateofservice { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter 10 digit valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number"), Display(Name = " ")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address"), Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string Email { get; set; }
        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string HistoryOfIllness { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string MedicalHistory { get; set; }
        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Medication { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Allergies { get; set; }

        [MaybeNull]
        public decimal? Temp { get; set; }
        [MaybeNull]
        public decimal? HR { get; set; }
        [MaybeNull]
        public decimal? RR { get; set; }
        [MaybeNull]
        public int? BPs { get; set; }
        [MaybeNull]
        public int? BPd { get; set; }
        [MaybeNull]
        public decimal? O2 { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Pain { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Heent { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string CV { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Chest { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string ABD { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Extr { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Skin { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Neuro { get; set; }

        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Other { get; set; }
        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Diagnosis { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string TreatmentPlan { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string MedicationsDispended { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Procedure { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9,\'\s]*$", ErrorMessage = "Special characters are not allowed except comma and quote")]
        public string Followup { get; set; }

        public bool isFinaled { get; set; }
        public bool check { get; set; }
    }
}
