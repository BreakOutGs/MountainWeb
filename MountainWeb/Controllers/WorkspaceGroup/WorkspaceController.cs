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
        public async Task<IActionResult> Index(string SortBy, string SearchText, string UpDown )
        {
            WorkspaceViewModel viewModel;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var aims = _context.Aim.Include(aim => aim.Settings)
                .Include(aim => aim.TaskLists).ThenInclude(e => e.UserTasks)
                .Include(aims => aims.TaskLists).ThenInclude(l => l.Settings)
                .Where(aim => aim.ApplicationUserId == user.Id).ToList();
            if(SearchText!=""&&SearchText!=" " && SearchText != null)
            {
                var _aims = new List<Aim>();
                bool _AimWithText;
                foreach(var aim in aims)
                {
                    _AimWithText = false;
                    if(aim.Name.Contains(SearchText))
                    {
                        _AimWithText = true;
                        
                    }
                    else
                    {
                        foreach(TaskList list in aim.TaskLists)
                        {
                            if(list.Name.Contains(SearchText))
                            {
                                _AimWithText = true;
                                break;
                            }
                            else 
                                foreach(UserTask task in list.UserTasks)
                                {
                                    if (task.Name.Contains(SearchText)) 
                                    {
                                        _AimWithText = true;
                                        break;
                                    }
                                }
                        }
                    }
                    if (_AimWithText) _aims.Add(aim);
                }
                aims = _aims;
            }
            viewModel = new WorkspaceViewModel(aims);
            if (SortBy!=null&SortBy!="Без сортування" && SortBy != "Сортування")
            {
                switch (SortBy)
                {
                    case "Назва цілі":
                        if (UpDown == "Up")
                        {
                            viewModel.Aims = viewModel.Aims.OrderBy(a => a.Name).ToList();
                        }
                        else if (UpDown == "Down")
                        {
                            viewModel.Aims = viewModel.Aims.OrderByDescending(a => a.Name).ToList();
                        }
                            break;
                    case "Сумарний пріоритет":
                        if (UpDown == "Up")
                        {
                            viewModel.Aims = viewModel.Aims.OrderBy(a => a.SummaryPriority).ToList();
                        }
                        else if (UpDown == "Down")
                        {
                            viewModel.Aims = viewModel.Aims.OrderByDescending(a => a.SummaryPriority).ToList();
                        }
                        break;
                    case "Середній пріоритет":
                        if (UpDown == "Up")
                        {
                            viewModel.Aims = viewModel.Aims.OrderBy(a => a.AveragePriority).ToList();
                        }
                        else if (UpDown == "Down")
                        {
                            viewModel.Aims = viewModel.Aims.OrderByDescending(a => a.AveragePriority).ToList();
                        }
                        break;
                }
            }
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

       [HttpPost]
       public async Task<bool>  ChangeExpand(int ItemId, string ItemType, bool Expanded)
       {
            switch (ItemType)
            {
                case "Aim":
                    Url.Action("ChangeAimExpand", "Aim", new { id = ItemId, IsExpanded = Expanded });
                    break;
                case "TaskList":
                    Url.Action("ChangeTaskListExpand", "TaskList", new { id = ItemId, IsExpanded = Expanded });
                    break;
                default: return false;
                   
            } 
            return true;
       }





    }
}
