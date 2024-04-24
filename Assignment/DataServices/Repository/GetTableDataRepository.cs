using Assignment;
using DataModels.ViewModel;
using DataServices.Interface;

namespace DataServices.Repository
{
    public class GetTableDataRepository : IGetTableDataRepository
    {
        private readonly ApplicationDbContext _context;
        public GetTableDataRepository()
        {
            _context = new ApplicationDbContext();
        }
        public List<TableDataViewModel> GetTableData()
        {
            List<TableDataViewModel> model = new List<TableDataViewModel>();
            List<Department> departments = _context.Departments.ToList();
            var dataList = _context.Employees.ToList();
            foreach (var item in dataList)
            {
                TableDataViewModel data = new TableDataViewModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Age = item.Age,
                    Gender = item.Gender,
                    Department = departments.Where(m => m.Id == item.DeptId).FirstOrDefault().Name,
                    //Department = departments.Any(m => m.Id == item.Id) ? departments[item.Id] : null,
                    Education = item.Education,
                    Company = item.Company,
                    Experience = item.Experience,
                    Package = item.Package,
                };
                model.Add(data);
            }
            return model;
        }
        public List<Department> GetDepartmentList()
        {
            return _context.Departments.ToList();
        }
        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.FirstOrDefault(m => m.Id == id);
        }

        //add employee
        public int AddEmployee(EmployeeViewModel model)
        {
            List<Department> departments = _context.Departments.ToList();
            if (model != null && departments.Count > 0)
            {

                Employee employee = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Age = model.Age,
                    Gender = model.Gender,
                    DeptId = int.Parse(model.DepartmentId),
                    Education = model.Education,
                    Company = model.Company,
                    Experience = model.Experience,
                    Package = model.Package,
                };
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //edit employee
        public int editEmployee(EmployeeViewModel model)
        {
            List<Department> departments = _context.Departments.ToList();
            if (model != null && departments.Count > 0)
            {
                Employee employee = _context.Employees.FirstOrDefault(m => m.Id == model.Id);
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.Age = model.Age;
                employee.Gender = model.Gender;
                employee.DeptId = int.Parse(model.DepartmentId);
                employee.Education = model.Education;
                employee.Company = model.Company;
                employee.Experience = model.Experience;
                employee.Package = model.Package;
                _context.Employees.Update(employee);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // delete employee
        public int deleteEmployee(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(m => m.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
