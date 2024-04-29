using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;

namespace HalloDoc.Controllers.Access
{
    [AuthorizationRepository("Admin,Physician")]

    public class AccessController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public AccessController(IunitOfWork unit)
        {
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
            var rolelist = _unitOfWork.tableData.GetRoleList();
            var model = new AccessRoleViewModel();
            model.AspNetRoleList = _unitOfWork.tableData.GetAspNetRoleList();
            model.rolelist = rolelist;
            if (accounttype != 0)
            {
                var accounttypemenulist = _unitOfWork.tableData.GetMenuListByAccountType(accounttype);
                model.menulist = accounttypemenulist;
            }
            else
            {
                var menulist = _unitOfWork.tableData.GetMenuList();
                model.menulist = menulist;
            }
            if (roleid != 0)
            {
                var role = _unitOfWork.tableData.GetRoleById(roleid);
                var rolemenu = _unitOfWork.tableData.GetRoleMenuListByRoleId(role.RoleId);
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
                var accounttypemenulist = _unitOfWork.tableData.GetMenuListByAccountType(accounttype);
                model.menulist = accounttypemenulist;
            }
            else
            {
                var menulist = _unitOfWork.tableData.GetMenuList();
                model.menulist = menulist;
            }
            return PartialView("MenuFilterCheckbox", model);
        }

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
                _unitOfWork.Add.AddRole(role);
                foreach (var item in selectedmenu)
                {
                    var rolemenu = new RoleMenu
                    {
                        MenuId = item
                    };
                    rolemenu.RoleId = role.RoleId;

                    _unitOfWork.Add.AddRoleMenu(rolemenu);
                }
                _unitOfWork.Add.SaveChangesDB();
                TempData["success"] = "Role Created Successfully!";
            }
            else
            {
                var role = _unitOfWork.tableData.GetRoleById(roleid);
                role.Name = rolename;
                role.AccountType = (short)accounttype;
                role.ModifiedBy = adminname;
                role.ModifiedDate = DateTime.Now;
                _unitOfWork.UpdateData.UpdateRole(role);
                int[]? roleMenus = _unitOfWork.tableData.GetRoleMenuListByRoleId(roleid).Select(s => s.MenuId).ToArray();
                IEnumerable<int> menusToDelete = roleMenus.Except(selectedmenu);
                foreach (var menuToDelete in menusToDelete)
                {
                    RoleMenu? roleMenu = _unitOfWork.tableData.GetRoleMenuListByRoleId(roleid).Where(r => r.MenuId == menuToDelete).FirstOrDefault();
                    if (roleMenu != null)
                    {
                        _unitOfWork.RemoveData.RemoveRoleMenu(roleMenu);
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
                    _unitOfWork.Add.AddRoleMenu(roleMenu);
                }
                _unitOfWork.Add.SaveChangesDB();
                TempData["success"] = "Role Updated Successfully!";
            }
            return RedirectToAction("AccessRole");
        }
        public IActionResult DeleteRole(int roleid)
        {
            if (roleid != 0)
            {
                Role role = _unitOfWork.tableData.GetRoleById(roleid);
                List<RoleMenu> roleMenu = _unitOfWork.tableData.GetRoleMenuListByRoleId(roleid);
                foreach (var item in roleMenu)
                {
                    _unitOfWork.RemoveData.RemoveRoleMenu(item);
                }
                _unitOfWork.RemoveData.RemoveRole(role);
                _unitOfWork.Add.SaveChangesDB();
            }
            TempData["success"] = "Role Deleted Successfully!";
            return RedirectToAction("AccessRole");
        }
        public IActionResult UserAccess()
        {
            var rolelist = _unitOfWork.tableData.GetRoleList();
            List<AccessViewModel> model = new List<AccessViewModel>();
            var aspuser = _unitOfWork.tableData.GetAspNetUserListAdminPhysician();
            foreach (var user in aspuser)
            {
                var access = new AccessViewModel();
                access.Phone = user.PhoneNumber;
                if (user.AspNetUserRoles.Count() > 0)
                {
                    if (user.AspNetUserRoles.FirstOrDefault(m => m.UserId == user.Id).RoleId == 1.ToString())
                    {
                        var admin = _unitOfWork.tableData.GetAdminByAspNetUserId(user.Id);
                        access.Accounttype = 1.ToString();
                        access.Status = admin.Status;
                        access.Name = admin.FirstName + admin.LastName;
                        access.OpenRequest = _unitOfWork.tableData.GetRequestList().Where(m => m.Status != 10 && m.Status != 11).Count().ToString();
                        access.AdminId = admin.AdminId;
                    }
                    if (user.AspNetUserRoles.FirstOrDefault(m => m.UserId == user.Id).RoleId == 2.ToString())
                    {
                        var physician = _unitOfWork.tableData.GetPhysicianByAspNetUserId(user.Id);
                        access.Status = physician.Status;
                        access.Accounttype = 2.ToString();
                        access.Name = physician.FirstName + physician.LastName;
                        access.OpenRequest = _unitOfWork.tableData.GetRequestList().Where(m => m.PhysicianId == physician.PhysicianId && m.Status < 7 && m.Status != 3).ToList().Count().ToString();
                        access.physicianId = physician.PhysicianId;
                    }
                }
                model.Add(access);
            }
            return View(model);
        }
        public IActionResult AdminProfileFromUserAccess(int adminid)
        {
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(admin.AspNetUserId);
            var rolelist = _unitOfWork.tableData.GetAspNetRoleList();
            var regionlist = _unitOfWork.tableData.GetRegionList();
            var adminregionlist = _unitOfWork.tableData.GetAdminRegionListByAdminId(adminid);
            var model = new UserAllDataViewModel
            {
                UserName = aspnetuser.UserName,
                password = aspnetuser.PasswordHash,
                status = admin.Status,
                role = rolelist,
                firstname = admin.FirstName,
                lastname = admin.LastName,
                email = admin.Email,
                confirmationemail = admin.Email,
                phonenumber = admin.Mobile,
                regionlist = regionlist,
                address1 = admin.Address1,
                address2 = admin.Address2,
                city = admin.City,
                zip = admin.Zip,
                alterphonenumber = admin.AltPhone,
                adminregionlist = adminregionlist,
                check = false,
            };
            return PartialView("../Admin/AdminProfile", model);
        }
    }
}
