using HalloDoc.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class registrationViewModel
    {
        public RequestClient RequestClient { get; set; }

        [Column(TypeName = "character varying")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Enter Your Email")]
        [StringLength(256)]
        public string? Email { get; set; }

        [Required]
        [Compare("PasswordHash")]

        public string? ConfirmPassword { get; set; }
    }
}
