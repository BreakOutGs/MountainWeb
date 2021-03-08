using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models;
using MountainWeb.Models.AimViewModels;
using MountainWeb.Models.TaskListViewModels;
using MountainWeb.Models.UserTaskViewModels;

namespace MountainWeb.Controllers
{
    public class WorkspaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkspaceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Workspace
        public async Task<IActionResult> Index()
        {
            WorkspaceViewModel viewModel;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var aims = _context.Aim.Include(aim => aim.TaskLists).ThenInclude(e => e.UserTasks).Where(aim => aim.ApplicationUserId == user.Id).ToList();
            viewModel = new WorkspaceViewModel(aims);
            return View(viewModel);
        }

        // GET: Workspace/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aim = await _context.Aim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aim == null)
            {
                return NotFound();
            }

            return View(aim);
        }

       


       
       
    }
}
