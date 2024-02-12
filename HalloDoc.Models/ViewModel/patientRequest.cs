using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class patientRequest
    {

        [StringLength(100)]
        [Required(ErrorMessage = "Enter First Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Enter Last Name")]
        public string? LastName { get; set; }

        [Column(TypeName = "character varying")]
        public string? Password { get; set; }




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

    }
}
