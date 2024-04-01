using HalloDoc.DataModels;

namespace Services.ViewModels
{
    public class AccessViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Physician> physicianlist { get; set; }
        public List<Role> rolelist { get; set; }
        public List<RoleMenu> rolemenulist { get; set; }
        public List<RoleMenu> selectedrolemenulist { get; set; }
        public List<Menu> menulist { get; set; }

        public enum accounttype
        {
            All,
            Admin,
            Physician,
            Patient
        }

        public string Accounttypename(int by)
        {
            string By = ((accounttype)by).ToString();
            return By;
        }
    }
}
