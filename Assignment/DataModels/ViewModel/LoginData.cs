using System.ComponentModel.DataAnnotations;

namespace DataModels.ViewModel
{
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
