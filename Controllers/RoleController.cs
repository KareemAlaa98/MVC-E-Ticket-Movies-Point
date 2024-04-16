using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies_Point.ViewModels;

namespace Movies_Point.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole(userRoleVM.Name);
                var result = await roleManager.CreateAsync(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else { ModelState.AddModelError(string.Empty, "Invalid Role"); }
            }
            return View(userRoleVM);
        }
    }
}
