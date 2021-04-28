using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.EventLogViewModels;
using MountainWeb.Models.RoleViewModels;

namespace MountainWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminPanelController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowRolesList()
        {
            return View("Roles/RolesList",_roleManager.Roles.ToList());
        }

        public IActionResult Create() => View("Roles/Create");

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
        }
        return RedirectToAction("Index");
    }

    public IActionResult UserList() => View(_userManager.Users.ToList());

    public async Task<IActionResult> Edit(string userId)
    {
        // получаем пользователя
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // получем список ролей пользователя
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View("Roles/Edit",model);
        }

        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
        // получаем пользователя
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // получем список ролей пользователя
            var userRoles = await _userManager.GetRolesAsync(user);
            // получаем все роли
            var allRoles = _roleManager.Roles.ToList();
            // получаем список ролей, которые были добавлены
            var addedRoles = roles.Except(userRoles);
            // получаем роли, которые были удалены
            var removedRoles = userRoles.Except(roles);

            await _userManager.AddToRolesAsync(user, addedRoles);

            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("UserList");
        }

        return NotFound();
    }
        public IActionResult ShowEventLogList(string EventType=null, string EventSearchString = null)
        {
            ICollection<ShowEventLogViewModel> logsViewModel = new List<ShowEventLogViewModel>();
            List<EventLog> _logs;
            if (EventType != null && EventType!="Всі")
            {
                 _logs = _context.EventLogs.Where(l=>l.EventType == (EventTypes)Enum.Parse(typeof(EventTypes), EventType)).ToList();
                ViewData["EventType"] = EventType;
            }
            else { _logs = _context.EventLogs.ToList();
                ViewData["EventType"] = "Всі";
            }
            if (EventSearchString != null) _logs = _logs.Where(l => l.Message.Contains(EventSearchString)).ToList();
            _logs.Reverse();
            foreach (var log in _logs) logsViewModel.Add(new ShowEventLogViewModel(log));
        
            return View("EventLogs/EventLogsList", logsViewModel);
        }
}
}

