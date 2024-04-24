using Assignment.Models;
using DataModels.ViewModel;
using DataServices.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetTableDataRepository _data;

        public HomeController(ILogger<HomeController> logger, IGetTableDataRepository loginrepo)
        {
            _logger = logger;
            _data = loginrepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetTableData()
        {
            List<TableDataViewModel> data = _data.GetTableData();
            return PartialView("_tableData", data);
        }

        //add employee methods
        public IActionResult AddEmployeeModel()
        {
            EmployeeViewModel model = new EmployeeViewModel
            {
                DepartmentList = _data.GetDepartmentList(),
            };
            return PartialView("_addEmployee", model);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel model)
        {
            var success = _data.AddEmployee(model);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //edit employee methods
        public IActionResult EditEmployee(int id)
        {
            var employee = _data.GetEmployeeById(id);
            EmployeeViewModel model = new EmployeeViewModel
            {
                Id = id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Age = employee.Age,
                Gender = employee.Gender,
                DepartmentId = employee.DeptId.ToString(),
                Education = employee.Education,
                Company = employee.Company,
                Experience = employee.Experience,
                Package = employee.Package,
                DepartmentList = _data.GetDepartmentList(),
            };
            return PartialView("_editEmployee", model);
        }
        [HttpPost]
        public IActionResult EditEmployee(EmployeeViewModel model)
        {
            var success = _data.editEmployee(model);
            return RedirectToAction("Index");
        }

        // delete employee
        public IActionResult DeleteEmployee(int id)
        {
            var success = _data.deleteEmployee(id);
            return RedirectToAction("Index");
        }
    }
}