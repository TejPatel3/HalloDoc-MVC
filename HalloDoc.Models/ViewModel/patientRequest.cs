using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "character varying")]
        public string? Password { get; set; }

        [Compare("Password")]
        [Column(TypeName = "character varying")]
        public string? ConfirmPassword { get; set; }

        //[Column("strMonth")]
        //[StringLength(20)]
        //public string? StrMonth { get; set; }

        //[Column("intYear")]
        //public int? IntYear { get; set; }

        //[Column("intDate")]
        //public int? IntDate { get; set; }
        [Required]
        [DataType(DataType.Date)]

        public DateTime? BirthDate { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "Please enter your Email Address"), Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string? Email { get; set; }

        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number"), Display(Name = " ")]
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
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Enter a valid 5-digit zip code")]
        //[Required(ErrorMessage = "Zip Code is required")]
        public string? ZipCode { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public int? RegionId { get; set; }

        [StringLength(500)]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9 ]+$", ErrorMessage = "Use letters only please")]
        public string? Notes { get; set; }

        public List<IFormFile?> Upload { get; set; }
    }
}


//public int RequestId { get; set; }

//[DefaultValue(2)]
//public int RequestTypeId { get; set; }

//public int? UserId { get; set; }

//[Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
//[StringLength(100)]
//[RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
//public string? FirstName { get; set; }

//[StringLength(100)]
//[Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
//[RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
//public string? LastName { get; set; }


//[StringLength(23)]
//[RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
//[Required(ErrorMessage = "Plese enter your Phone Number"), Display(Name = " ")]
//public string? PhoneNumber { get; set; }

//[StringLength(50)]
//[Required(ErrorMessage = "Please enter your Email Address"), Display(Name = "Email Address")]
//[RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]

////[RegularExpression(@"^[^@\\s]+@[^@\\s]+\\.(com|net|org|gov)$", ErrorMessage = "Enter valid email address")]
//public string? Email { get; set; }

//[Required(ErrorMessage = "Create Password to proceed")]
//public string? Password { get; set; }

//[Compare("Password", ErrorMessage = "Does not match with Password")]
//public string? ConfirmPassword { get; set; }

//[DefaultValue(1)]
//public short Status { get; set; }

//[StringLength(500, ErrorMessage = "Maximum allowed length is 500")]
//public string? Symptoms { get; set; }

//[Required]
//[DataType(DataType.Date)]
//public DateTime DOB { get; set; }

//[Required(ErrorMessage = "Street is required")]
//[StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid Street")]
////[RegularExpression(@"^[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
//[RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
//public string? Street { get; set; }

//[Required(ErrorMessage = "City is required")]
//[StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
//[RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
//public string? City { get; set; }

//[Required(ErrorMessage = "State is required")]
//[StringLength(50, MinimumLength = 2, ErrorMessage = "Enter valid State")]
//[RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid State name")]
////[RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Enter a valid state abbreviation (e.g., NY, CA)")]
//public string? State { get; set; }

//[Required(ErrorMessage = "Zip Code is required")]
//[StringLength(10, ErrorMessage = "Enter valid Zip Code")]
//[RegularExpression(@"^\d{5}$", ErrorMessage = "Enter a valid 5-digit zip code")]
//public string? ZipCode { get; set; }

//[StringLength(100, ErrorMessage = "Maximum length is 100")]
//public string? RoomSuit { get; set; }

//public string? FileName { get; set; }

//public IFormFile? Upload { get; set; }

//public DateTime CreatedDate { get; set; }