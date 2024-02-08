using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models.ViewModel
{
    public class registrationViewModel
    {
        [Column(TypeName = "character varying")]
        public string? PasswordHash { get; set; }

        [StringLength(256)]
        public string? Email { get; set; }

        [Required]
        [Compare("PasswordHash")]

        public string? ConfirmPassword { get; set; }



    }
}
