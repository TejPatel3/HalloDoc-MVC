using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;

namespace HalloDoc.Controllers.Access
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IunitOfWork _unitOfWork;
        public AccessController(ApplicationDbContext context, IunitOfWork unit)
        {
            _context = context;
            _unitOfWork = unit;
        }
        public IActionResult AccessRole()
        {
            var rolelist = _unitOfWork.tableData.GetRoleList();
            var model = new AccessRoleViewModel
            {
                rolelist = rolelist
            };
            return View(model);
        }

        public IActionResult CreateRole(int accounttype, int roleid)
        {
            var rolelist = _context.Roles.ToList();
            var model = new AccessRoleViewModel();
            model.AspNetRoleList = _unitOfWork.tableData.GetAspNetRoleList();
            model.rolelist = rolelist;
            if (accounttype != 0)
            {
                var accounttypemenulist = _context.Menus.Where(m => m.AccountType == accounttype).ToList();
                model.menulist = accounttypemenulist;
            }
            else
            {
                var menulist = _context.Menus.ToList();
                model.menulist = menulist;
            }
            if (roleid != 0)
            {
                var role = _context.Roles.FirstOrDefault(m => m.RoleId == roleid);
                var rolemenu = _context.RoleMenus.Where(m => m.RoleId == role.RoleId).ToList();
                model.RoleName = role.Name;
                model.RoleId = role.RoleId.ToString();
                model.selectedrolemenulist = rolemenu;
                model.AccountType = role.AccountType;
            }
            return View(model);
        }

        public IActionResult MenuFilterCheck(int accounttype)
        {
            var model = new AccessRoleViewModel();
            if (accounttype != 0)
            {
                var accounttypemenulist = _context.Menus.Where(m => m.AccountType == accounttype).ToList();
                model.menulist = accounttypemenulist;
            }
            else
            {
                var menulist = _context.Menus.ToList();
                model.menulist = menulist;
            }
            return PartialView("MenuFilterCheckbox", model);
        }

        //public IActionResult MenuFilterCheckbox(AccessRoleViewModel model)
        //{
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult CreateRole(string rolename, int accounttype, int[] selectedmenu, int roleid)
        {
            var adminname = HttpContext.Session.GetString("AdminName");
            if (roleid == 0)
            {
                var role = new Role();
                var bit = new BitArray(1);
                bit[0] = false;
                role.Name = rolename;
                role.AccountType = (short)accounttype;
                role.CreatedBy = adminname;
                role.CreatedDate = DateTime.Now;
                role.IsDeleted = bit;

                _context.Roles.Add(role);
                _context.SaveChanges();

                foreach (var item in selectedmenu)
                {
                    var rolemenu = new RoleMenu
                    {
                        MenuId = item
                    };
                    rolemenu.RoleId = role.RoleId;

                    _context.RoleMenus.Add(rolemenu);
                }
                _context.SaveChanges();
                TempData["success"] = "Role Created Successfully!";
            }
            else
            {
                var role = _unitOfWork.tableData.GetRoleById(roleid);
                role.Name = rolename;
                role.AccountType = (short)accounttype;
                role.ModifiedBy = adminname;
                role.ModifiedDate = DateTime.Now;

                _context.Roles.Update(role);
                _context.SaveChanges();
                int[]? roleMenus = _context.RoleMenus.Where(r => r.RoleId == roleid).Select(s => s.MenuId).ToArray();

                IEnumerable<int> menusToDelete = roleMenus.Except(selectedmenu);

                foreach (var menuToDelete in menusToDelete)
                {
                    RoleMenu? roleMenu = _context.RoleMenus.Where(r => r.RoleId == roleid && r.MenuId == menuToDelete).FirstOrDefault();

                    if (roleMenu != null)
                    {
                        _context.Remove(roleMenu);
                    }

                }

                IEnumerable<int> menusToAdd = selectedmenu.Except(roleMenus);

                foreach (var menuToAdd in menusToAdd)
                {
                    RoleMenu roleMenu = new RoleMenu
                    {
                        RoleId = roleid,
                        MenuId = menuToAdd,
                    };
                    _context.Add(roleMenu);
                }

                _context.SaveChanges();


                TempData["success"] = "Role Updated Successfully!";
            }


            return RedirectToAction("AccessRole");
        }
        public IActionResult DeleteRole(int roleid)
        {
            if (roleid != 0)
            {
                Role role = _context.Roles.FirstOrDefault(m => m.RoleId == roleid);
                List<RoleMenu> roleMenu = _context.RoleMenus.Where(m => m.RoleId == roleid).ToList();
                foreach (var item in roleMenu)
                {
                    _context.Remove(item);
                }
                _context.Remove(role);
                _context.SaveChanges();

            }
            TempData["success"] = "Role Deleted Successfully!";
            return RedirectToAction("AccessRole");
        }
        public IActionResult UserAccess()
        {
            var rolelist = _context.Roles.ToList();
            AccessViewModel model = new AccessViewModel
            {
                rolelist = rolelist
            };
            return View(model);
        }
    }
}
