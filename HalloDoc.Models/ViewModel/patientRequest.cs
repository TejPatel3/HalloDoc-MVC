using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HalloDoc.Models.ViewModel
{
    public class patientRequest
    {

        [Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string? LastName { get; set; }

        public string? Password { get; set; }

        [AllowNull]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "Please enter your Email Address"), Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string? Email { get; set; }

        [StringLength(23)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter 10 digit valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid Street")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
        //[Required(ErrorMessage = "Street is required")]
        public string? Street { get; set; }


        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        //[Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter valid State")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid State name")]
        //[Required(ErrorMessage = "State is required")]
        public string? State { get; set; }

        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 5-digit zip code")]
        //[Required(ErrorMessage = "Zip Code is required")]
        public string? ZipCode { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
        public int? RegionId { get; set; }

        [StringLength(500)]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9 ]+$", ErrorMessage = "Use letters only please")]
        public string? Notes { get; set; }
        [MaybeNull]
        public List<IFormFile?> Upload { get; set; }
    }
}

