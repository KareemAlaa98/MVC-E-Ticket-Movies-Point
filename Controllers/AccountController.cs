using Microsoft.AspNetCore.Mvc;
using Movies_Point.ViewModels;
using Movies_Point.Models;
using Movies_Point.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Movies_Point.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        ApplicationDbContext context;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ApplicationDbContext context)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(ApplicationUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                { 
                    UserName = userVM.UserName, 
                    Email = userVM.Email,
                    PasswordHash = userVM.Password,
                    Age = userVM.Age
                };
                
                var result = await userManager.CreateAsync(user, userVM.Password);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    
                    return View("Login");
                    
                    /////Automatically signin upon signing up and redirect to home page
                    //await signInManager.SignInAsync(user, true);
                    //return RedirectToAction("Index", "Home");
                }

                return View(userVM);
            }
            return View(userVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }  
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVM userVM)
        {
           if(ModelState.IsValid)
           {
               var user = await userManager.FindByNameAsync(userVM.UserName); 

               if(user != null)
               {
                   bool checkPassWord = await userManager.CheckPasswordAsync(user, userVM.Password);
                   if(checkPassWord)
                   {
                       await signInManager.SignInAsync(user, userVM.RememberMe);
                       return RedirectToAction("Index", "HOme");
                   }
                   // Invalid Password
                   ModelState.AddModelError("InvalidPwd", "Invalid Password");
               }
               else
               {
                   // Invalid Username
                   ModelState.AddModelError("InvalidPwd", "Invalid Password");
               }
           }
            return View(userVM);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            // Clear the cart after Logging out
            HttpContext.Session.Remove("StringMovies");

            return RedirectToAction("Index", "Home");
        }
    }
}
