using DataModels.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class registrationViewModel
    {
        public RequestClient RequestClient { get; set; }

        [Column(TypeName = "character varying")]
        [Required]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Enter Your Email")]
        [StringLength(256)]
        public string? Email { get; set; }

        [Required]
        [Compare("PasswordHash")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
