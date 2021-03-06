using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models;
using MountainWeb.Models.WorkspaceGroup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ShowWorkspace(int id, string SortBy, string SearchText, string UpDown)
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
               .ThenInclude(ut => ut.Settings)
               .FirstAsync();
            user.CurrentWorkspaceId = workspace.Id;
            _context.SaveChanges();
           
            viewModel = new WorkspaceViewModel(workspace);
         
            return View("Workspace_2", viewModel);
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
                    
                case "TaskList":
                    return RedirectToAction("ChangeTaskListExpand", "TaskList", new { id = ItemId, IsExpanded = Expanded });
                    //   RedirectToAction(Url.Action("ChangeTaskListExpand", "TaskList", new { id = ItemId, IsExpanded = Expanded }));
                    
                default: return null;

            }
            return null;
        }

        // TODO: get Workspaces page
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.CurrentWorkspaceId>0) {
                return RedirectToAction("ShowWorkspace", new { Id = user.CurrentWorkspaceId });
            }
            var workspaces = await _context.Workspaces.Where(ws => ws.ApplicationUserId == user.Id)
                .Include(ws=>ws.Aims)
                .ThenInclude(a=>a.TaskLists)
                .ThenInclude(a=>a.UserTasks)
                .ToListAsync();
           
            ChooseWorkspaceMV ViewModel = new ChooseWorkspaceMV() { Workspaces = workspaces };
            return View(ViewModel);
        }
        public IActionResult ShowCreateWorkspace(int id)
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
                .Include(w => w.Settings)
                .First();
            ChangeWorkspaceVM ViewModel = new ChangeWorkspaceVM(workspace);
            return View("Edit");
        }
        public async Task<IActionResult> EditWorkspaceAsync(ChangeWorkspaceVM changeViewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workspace = await _context.Workspaces.Where(w => w.Id == changeViewModel.Id).SingleAsync();
            if (user.Id == workspace.ApplicationUserId)
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
        /*  [HttpPost]
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

          }*/
        [HttpPost]
        public bool DeleteWorkspace(int Id)
        {
            var workspace = _context.Workspaces.Single(w => w.Id == Id);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (workspace.ApplicationUserId == userId)
            {
                _context.Workspaces.Remove(workspace);
                _context.SaveChanges();
                return false;
            }
            return true;

        }

        public IActionResult ExitFromWorkspace()
        {
            var user = _context.ApplicationUsers.Single(user => user.Id == _userManager.GetUserId(HttpContext.User));
            user.CurrentWorkspaceId = 0;
            _context.SaveChanges();
           return RedirectToAction("Index");
        }

        [HttpPost]
        public void ChangeCurrentAim(int _WorkspaceId, int _AimId)
        {
            var WorkspaceSettings = _context.WorkspaceSettings.Single(s => s.WorkspaceId == _WorkspaceId);
            WorkspaceSettings.CurrentAim = _AimId;
            _context.SaveChanges();
        }

    }
}
