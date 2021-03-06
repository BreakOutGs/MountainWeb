using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.TaskListViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Controllers.WorkspaceGroup
{
    public class TaskListController : Controller
    {

        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TaskListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTaskList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CreateTaskListViewModel createModel = new CreateTaskListViewModel();
            return View("CreateTaskList", createModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskList(int id, CreateTaskListViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var aim = await _context.Aim.FirstOrDefaultAsync(a => a.Id == id);
                TaskList listToAdd = new TaskList()
                {
                    Name = createModel.Name,
                    Description = createModel.Description,
                    AimId = id,
                    Aim = aim,
                };
                _context.EventLogs.Add(new EventLog()
                {
                    Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") edited TaskList(id: " + listToAdd.Id + ", name:" + listToAdd.Name + ")"),
                    EventType = EventTypes.TasksListCreated
                });
                _context.TaskList.Add(listToAdd);
                await _context.SaveChangesAsync();

                listToAdd.Settings = new TaskListSettings()
                {
                    TaskList = listToAdd,
                    TaskListId = listToAdd.Id
                };
                _context.TaskListSettings.Add(listToAdd.Settings);
                _context.TaskList.Update(listToAdd);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Workspace", "");
            }
            return View(createModel);
        }

        public IActionResult EditTaskList(int? id)
        {
            if (id == null || !TaskListExists((int)id))
            {
                return NotFound();
            }
            var list = _context.TaskList.Single(l => l.Id == id);
            var userid = _userManager.GetUserId(HttpContext.User);
            EditTaskListViewModel editModelView = new EditTaskListViewModel()
            {
                AppUseId = userid,
                Name = list.Name,
                Description = list.Description,
                Id = list.Id
            };
            return View("EditTaskList", editModelView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskList(int id, EditTaskListViewModel editViewModel)
        {
            if (id != editViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (editViewModel.AppUseId != user.Id) return NotFound();
                var list = await _context.TaskList.SingleAsync(e => e.Id == editViewModel.Id);
                {//changing aimModel props by new values from modelview
                    list.Name = editViewModel.Name;
                    list.Description = editViewModel.Description;
                }

                try
                {
                    _context.EventLogs.Add(new EventLog()
                    {
                        Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") edited TaskList(id: " + list.Id + ", name:" + list.Name + ")"),
                        EventType = EventTypes.TaskEdited
                    });
                    _context.Update(list);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(list.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Workspace", "");
            }
            return View(editViewModel);
        }

        public async Task<IActionResult> DeleteTaskList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var list = await _context.TaskList.Include(l => l.Aim)
                .ThenInclude(a=>a.Workspace)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (list == null)
            {
                return NotFound();
            }
            if (list.Aim.Workspace.ApplicationUserId != user.Id)
            {
                return NotFound();
            }

            var deleteViewModel = new DeleteTaskListViewModel()
            {
                Name = list.Name,
                Id = list.Id
            };

            return View("DeleteTaskList", deleteViewModel);
        }

        // POST: Workspace/Delete/5
        [HttpPost, ActionName("DeleteTaskList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskListConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var list = await _context.TaskList.Include(l => l.UserTasks).Include(l => l.Settings).SingleAsync(l => l.Id == id);
            _context.TaskListSettings.Remove(list.Settings);
            _context.TaskList.Remove(list);
            _context.EventLogs.Add(new EventLog()
            {
                Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") removes TaskList(id: " + list.Id + ", name:" + list.Name + ")"),
                EventType = EventTypes.TasksListRemoved
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Workspace", "");
        }

        private bool TaskListExists(int id)
        {
            return _context.TaskList.Any(e => e.Id == id);
        }
        public void ChangeTaskListExpand(int id, bool IsExpanded)
        {
            var taskListSettings = _context.TaskListSettings.First(s => s.TaskListId == id);
            taskListSettings.Expanded = IsExpanded;
            _context.Update(taskListSettings);
            _context.SaveChanges();
        }

        public TaskListSettings TaskListSettingsIsExistOrCreate(int listId)
        {
            if (!_context.TaskListSettings.Any(s => s.TaskListId == listId))
            {
                var list = _context.TaskList.First(list => list.Id == listId);
                list.Settings = new TaskListSettings()
                {
                    TaskList = list,
                    TaskListId = list.Id
                };

                _context.TaskListSettings.Add(list.Settings);
                _context.TaskList.Update(list);
                _context.SaveChanges();
                return list.Settings;
            }
            else return _context.TaskListSettings.First(s => s.TaskListId == listId);
        }


    }
}
