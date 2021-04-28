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
using MountainWeb.Models.WorkspaceGroup;

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
        public async Task<IActionResult> ShowWorkspace(int id, string SortBy, string SearchText, string UpDown )
        {
            WorkspaceViewModel viewModel;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workspace = await _context.Workspaces.Where(w => w.Id == id)
                .Include(wspace => wspace.Settings)
                .Include(wspace => wspace.Aims)
                .ThenInclude(aim => aim.Settings)
                .Include(wspace => wspace.Aims)
                .ThenInclude(aim => aim.TaskLists)
                .ThenInclude(tl => tl.Settings)
                .Include(wspace => wspace.Aims)
                .ThenInclude(aim => aim.TaskLists)
               .ThenInclude(tl => tl.UserTasks)
               .ThenInclude(ut => ut.Settings).FirstAsync();
          /*  if (SearchText!=""&&SearchText!=" " && SearchText != null)
            {
                var _aims = new List<Aim>();
                bool _AimWithText;
                foreach(var aim in workspace.Aims)
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
            } */
            viewModel = new WorkspaceViewModel(workspace);
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
            return View("Workspace",viewModel);
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
        public IActionResult ChangeExpand(int ItemId, string ItemType, bool Expanded)
        {

            switch (ItemType)
            {
                case "Aim":
                    return RedirectToAction("ChangeAimExpand", "Aim", new { id = ItemId, IsExpanded = Expanded });
                    break;
                case "TaskList":
                    return RedirectToAction("ChangeTaskListExpand", "TaskList", new { id = ItemId, IsExpanded = Expanded });
                    //   RedirectToAction(Url.Action("ChangeTaskListExpand", "TaskList", new { id = ItemId, IsExpanded = Expanded }));
                    break;
                default: return null;

            }
            return null;
        }

        // TODO: get Workspaces page
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workspaces = await _context.Workspaces.Where(ws => ws.ApplicationUserId == user.Id).ToListAsync();
            ChooseWorkspaceMV ViewModel = new ChooseWorkspaceMV() { Workspaces = workspaces };
            return View(ViewModel);
        }
        public  IActionResult ShowCreateWorkspace(int id)
        {
            CreateVM ViewModel = new CreateVM();
            return View("Create", ViewModel); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWorkspace(CreateVM ViewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Workspace workspace = new Workspace()
            {
                Name = ViewModel.Name,
                ApplicationUserId = user.Id
            };
            await _context.Workspaces.AddAsync(workspace);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Workspace", "");
        }
        public IActionResult ShowEditWorkspace(int id)
        {
            Workspace workspace = _context.Workspaces.Where(w => w.Id == id)
                .Include(w=>w.Settings)
                .First();
            ChangeWorkspaceVM ViewModel = new ChangeWorkspaceVM(workspace);
            return View("Edit");
        }
        public async Task<IActionResult> EditWorkspaceAsync(ChangeWorkspaceVM changeViewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workspace = await _context.Workspaces.Where(w => w.Id == changeViewModel.Id).SingleAsync();
            if(user.Id == workspace.ApplicationUserId)
            {
                workspace.Name = changeViewModel.Name;
                _context.Workspaces.Update(workspace);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction("Index", "Workspace", "");
        }
        public IActionResult ShowDeleteWorkspace(int id)
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWorkspace(int id)
        {
            var workspace = await _context.Workspaces.SingleAsync(w=>w.Id==id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (workspace.ApplicationUserId == user.Id)
            {
                _context.Workspaces.Remove(workspace);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction("Index", "Workspace", "");

        }
        // TODO: create Workspace
        // TODO: delete workspace 



    }
}
