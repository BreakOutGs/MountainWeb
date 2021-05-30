using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.AppUser;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MountainWeb.Models.AppUser.Edit;
using MimeKit;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace MountainWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment environment, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = environment;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var appuser = _context.Users.First(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var Viewmodel = new IndexVM(appuser);
            return View(Viewmodel);
        }

        public IActionResult EditProfile()
        {
            return RedirectToAction("EditProfileInfo");
        }
      
        public IActionResult EditEmail()
        {
            var appuser = _context.Users.First(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var Viewmodel = new EditEmailVM() { Email = appuser.Email};
            return View("Edit/EditEmail", Viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeEmail(EditEmailVM ViewModel)
        {
            var user = _context.Users.Single(u=>u.Id == _userManager.GetUserId(HttpContext.User));
            if (user == null) return NotFound();
            user.Email = ViewModel.Email;
            user.EmailConfirmed = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailConfirmationAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, user.Email);
            var callbackUrl = Url.Action("ConfirmEmail", "User", new { id = user.Id, user.Email, code = code }, null);
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mountain", "mountainweb2021@gmail.com"));
            message.To.Add(new MailboxAddress(name: user.UserName, address: user.Email));
            message.Subject = "Email Confirmation from Mountain";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Підтвердіть свою пошти натиснувши <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>це посилання</a>."
            };
            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("mountainweb2021@gmail.com", "Upalid15_norma5");
                client.Send(message);
                client.Disconnect(true);
            }
            return View("EmailConfirmation");
        }

        public async Task<IActionResult> EmailConfirm(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("EditEmail");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("EditEmail");
        }
            
        public IActionResult EditProfileInfo()
        {
            var appuser = _context.Users.First(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var Viewmodel = new EditProfileInfoVM() { Name = appuser.UserName };
            return View("Edit/EditProfileInfo", Viewmodel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeProfileInfo(EditProfileInfoVM ViewModel)
        {
            var user = _context.Users.Single(u => u.Id == _userManager.GetUserId(HttpContext.User));
            if (user == null) return NotFound();
            user.UserName = ViewModel.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult EditAvatar()
        {
            var appuser = _context.Users.First(u => u.Id == _userManager.GetUserId(HttpContext.User));
            var Viewmodel = new EditAvatarVM() { AvatarPath = appuser.AvatarPath};
            return View("Edit/EditAvatar", Viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeAvatar(IFormFile file)
        {
            ApplicationUser user = _context.Users.Single(u=>u.Id == _userManager.GetUserId(HttpContext.User));
            if (file == null) return RedirectToAction("");
            if (user.AvatarPath != null)
            {
                string existfile = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", user.AvatarPath);
                System.IO.File.Delete(existfile);

            }
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "avatar");
            var filePath = Path.Combine(uploads, file.FileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            user.AvatarPath = file.FileName; // Set the file name
            _context.SaveChanges();
            return RedirectToAction("EditAvatar");
        }

        public IActionResult EditPassword()
        {
            var Viewmodel = new EditPasswordVM();
            return View("Edit/EditPassword", Viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(EditPasswordVM ViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(user,
                    ViewModel.OldPassword, ViewModel.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index");
            }

            return RedirectToAction("EditPassword");

        }



        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }

    


}
