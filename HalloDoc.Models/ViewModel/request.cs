using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models.ViewModel
{
    public class request
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Your First Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Your First Name")]
        public string? rFirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string? rLastName { get; set; }

        [StringLength(50)]
        [Required]
        public string? rEmail { get; set; }

        [StringLength(50)]
        [Required]
        public string? rPhoneNumber { get; set; }

        //[Column("strMonth")]
        //[StringLength(20)]
        //public string? StrMonth { get; set; }

        //[Column("intYear")]
        //public int? IntYear { get; set; }

        //[Column("intDate")]
        //public int? IntDate { get; set; }
        [Required]
        public string? BirthDate { get; set; }


        [StringLength(50)]
        [Required]
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

    }
}
