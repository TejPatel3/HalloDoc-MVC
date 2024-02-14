using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class request
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Your First Name")]
        public string? rFirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Enter Last Name")]
        public string? rLastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Enter Your Email")]
        public string? rEmail { get; set; }

        [StringLength(50)]
        [Required]
        public string? rPhoneNumber { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Enter First Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Enter Last Name")]
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


        [Required(ErrorMessage = "Enter Your Email")]
        [StringLength(256)]
        public string? Email { get; set; }

        [StringLength(50)]
        [Required]
        public string? PhoneNumber { get; set; }
        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(100)]

        public string? City { get; set; }

        [StringLength(100)]

        public string? State { get; set; }

        [StringLength(10)]

        public string? ZipCode { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public int? RegionId { get; set; }


        [StringLength(500)]
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
