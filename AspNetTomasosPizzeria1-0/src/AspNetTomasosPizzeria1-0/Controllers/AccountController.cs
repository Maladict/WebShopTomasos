using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AspNetTomasosPizzeria1_0.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private TomasosContext _context;
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public AccountController(TomasosContext context, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }


        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Kund model)
        {
            if (ModelState.IsValid)
            {
                //Create Kund
                model.Role = "RegularUser";
                _context.Kund.Add(model);
                _context.SaveChanges();

                //Create appuser
                var user = new AppUser() {UserName = model.AnvandarNamn, KundId = model.KundId};
                var userResult = await _userManager.CreateAsync(user, model.Losenord);

                //Add user role
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (userResult.Succeeded && roleResult.Succeeded)
                {

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

            }

            // If we got this far, something failed, redisplay form
            return View("Error", model);
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LogIn(Kund user)
        {
            var result =
                await
                    _signInManager.PasswordSignInAsync(user.AnvandarNamn, user.Losenord, isPersistent: true,
                        lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }


        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ShowAccount()
        {
            var kund = GetUser().Result;

            if (kund != null)
            {
                return View(kund);
            }
            else
            {
                return View("Error", "Shared");
            }
        }

        public IActionResult EditAccount(int id)
        {
            var user = _context.Kund.SingleOrDefault(x => x.KundId == id);

            var roles = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Administratör",
                    Value="Administrator"
                },

                new SelectListItem()
                {
                    Text = "Premium User",
                    Value="PremiumUser"
                },

                new SelectListItem()
                {
                    Text = "Regular User",
                    Value="RegularUser"
                },
            };

            foreach (var role in roles)
            {
                if (user.Role == role.Value)
                {
                    role.Selected = true;
                }
            }

            ViewBag.Roles = roles;

            if (user != null)
            {
                return View(user);
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(Kund updatedUser)
        {
            if (ModelState.IsValid)
            {
                //Update identity user
                AppUser user = await _userManager.FindByNameAsync(updatedUser.AnvandarNamn);

                if (user != null)
                {                    
                    user.PasswordHash = _passwordHasher.HashPassword(user, updatedUser.Losenord);
                    var result = await _userManager.UpdateAsync(user);

                    //Update role                                    
                    var oldRoleName = _context.Kund.SingleOrDefault(x => x.KundId == updatedUser.KundId).Role;
                    var roleResult = await _userManager.RemoveFromRoleAsync(user, oldRoleName);

                    roleResult = await _userManager.AddToRoleAsync(user, updatedUser.Role);

                    //Update Kund                
                    var userToUpdate = _context.Kund.FirstOrDefault(x => x.KundId == updatedUser.KundId);

                    _context.Entry(userToUpdate).CurrentValues.SetValues(updatedUser);
                    _context.SaveChanges();

                    if (result.Succeeded && roleResult.Succeeded)
                    {
                        if (User.IsInRole("Administrator"))
                        {
                            return RedirectToAction("ViewUsers", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("ShowAccount", "Account");
                        }
                        
                    }                   
                }             
            }
            return View(updatedUser);
        }
    public async Task<Kund> GetUser()
        {

            var user = await GetCurrentUserAsync();
            var userId = user?.KundId;

            var kund = _context.Kund.SingleOrDefault(x => x.KundId == userId);


            return kund;
        }
}
}
