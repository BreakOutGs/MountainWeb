using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Controllers
{
    public class RolesController
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        private IActionResult View(List<IdentityRole> lists)
        {
            throw new NotImplementedException();
        }

       
    }
}
