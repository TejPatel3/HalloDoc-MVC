using Assignment;
using DataModels.ViewModel;

namespace DataServices.Interface
{
    public interface IGetTableDataRepository
    {
        //get table data method
        public List<TableDataViewModel> GetTableData();
        public List<Department> GetDepartmentList();
        public Employee GetEmployeeById(int id);

        // add employee
        public int AddEmployee(EmployeeViewModel model);

        // edit employee
        public int editEmployee(EmployeeViewModel model);

        //delete employee
        public int deleteEmployee(int id);
    }
}
