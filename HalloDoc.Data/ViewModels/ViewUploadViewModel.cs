using HalloDoc.DataModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class ViewUploadViewModel
    {
        public String? ConfirmationNumber { get; set; }

        public List<RequestWiseFile> wiseFiles { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public DateTime DOB { get; set; }

        public string PhoneNumber { get; set; }

        public int requestid { get; set; }

        [Required]
        public List<IFormFile?> Upload { get; set; }
    }
}
