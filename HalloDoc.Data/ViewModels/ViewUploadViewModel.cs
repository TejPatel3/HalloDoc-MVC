using HalloDoc.DataModels;
using Microsoft.AspNetCore.Http;

namespace Services.ViewModels
{
    public class ViewUploadViewModel
    {
        public String? ConfirmationNumber { get; set; }
        public List<RequestWiseFile> wiseFiles { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int requestid { get; set; }
        public List<IFormFile?> Upload { get; set; }

    }
}
