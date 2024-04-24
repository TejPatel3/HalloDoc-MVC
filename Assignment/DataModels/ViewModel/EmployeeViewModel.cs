using Assignment;
using System.ComponentModel.DataAnnotations;

namespace DataModels.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [RegularExpression(@"^[1-9][0-9]{0,1}$", ErrorMessage = "Age is not valid")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string DepartmentId { get; set; }

        [Required(ErrorMessage = "Education is required.")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [RegularExpression(@"^[1-9]{1,2}[.]{0,1}[0-9]{0,1}$", ErrorMessage = "Please enter digit only")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Package is required.")]
        [RegularExpression(@"^[1-9]{1,2}[.]{0,1}[0-9]{0,1}$", ErrorMessage = "Please enter digit only")]
        public string Package { get; set; }

        public List<Department> DepartmentList { get; set; }
    }
}
