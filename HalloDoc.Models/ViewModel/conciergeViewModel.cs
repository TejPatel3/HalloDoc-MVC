﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class conciergeViewModel
    {
        [Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]

        public string? rFirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]

        public string? rLastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Enter Your Email")]
        public string? rEmail { get; set; }

        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number"), Display(Name = " ")]
        public string? rPhoneNumber { get; set; }

        [Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string? LastName { get; set; }
        [Required]
        [Column(TypeName = "character varying")]
        public string? Password { get; set; }


        [Required]

        [Compare("Password")]

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
        [Required]

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid Street")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
        public string? Street { get; set; }
        [Required]

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string? City { get; set; }
        [Required]

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter valid State")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid State name")]
        public string? State { get; set; }
        [Required]

        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Enter a valid 5-digit zip code")]
        public string? ZipCode { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public int? RegionId { get; set; }

        [StringLength(500)]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9 ]+$", ErrorMessage = "Use letters only please")]
        public string? Notes { get; set; }

        //[StringLength(20)]
        //public string? NotiMobile { get; set; }

        //[StringLength(50)]
        //public string? NotiEmail { get; set; }


        //[Column(TypeName = "bit(1)")]
        //public BitArray? IsMobile { get; set; }


        //public short? CommunicationType { get; set; }

        //public short? RemindReservationCount { get; set; }

        //public short? RemindHouseCallCount { get; set; }

        //public short? IsSetFollowupSent { get; set; }

        //[Column("IP")]
        //[StringLength(20)]
        //public string? Ip { get; set; }

        //public short? IsReservationReminderSent { get; set; }

        //[Precision(9, 6)]
        //public decimal? Latitude { get; set; }

        //[Precision(9, 6)]
        //public decimal? Longitude { get; set; }

        //[ForeignKey("RegionId")]
        //[InverseProperty("RequestClients")]
        //public virtual Region? Region { get; set; }

        //[ForeignKey("RequestId")]
        //[InverseProperty("RequestClients")]
        //public virtual Request Request { get; set; } = null!;
        public List<IFormFile?> Upload { get; set; }
    }
}