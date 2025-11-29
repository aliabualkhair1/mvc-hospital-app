using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Hospital_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<UserInfo> user;

        public AdminController(UserManager<UserInfo> USER)
        {
            user = USER;
        }
        [Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<IActionResult> Show(string searchengine = "")
        {
            if (string.IsNullOrEmpty(searchengine))
            {
                var _users = await user.Users.ToListAsync();
                var _userWithRoles = new List<UserWithRoleViewModel>();
            }
            var filter = searchengine.Trim();
            var users = await user.Users.Where(p=>p.PhoneNumber.Trim().Contains(filter)).ToListAsync();
            var userWithRoles = new List<UserWithRoleViewModel>();

            foreach (var _user in users)
            {
                var roles = await user.GetRolesAsync(_user);
                userWithRoles.Add(new UserWithRoleViewModel
                {
                    UserId = _user.Id,
                    Phone=_user.PhoneNumber,
                    FirstName = _user.FirstName,
                    LastName= _user.LastName,
                    Email = _user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(userWithRoles);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var _user = await user.FindByIdAsync(userId);
            if (_user != null)
            {
                var currentRoles = await user.GetRolesAsync(_user);
                await user.RemoveFromRolesAsync(_user, currentRoles);
                await user.AddToRoleAsync(_user, newRole);
            }
            TempData["Success"] = "تم تغيير الدور بنجاح";
            return RedirectToAction("Show");
        }
   
    }
}

