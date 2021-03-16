using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.TaskListViewModels;

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
                    Aim = aim
                };


                _context.TaskList.Add(listToAdd);
                await _context.SaveChangesAsync();
                _context.eventLogs.Add(new EventLog()
                {
                    Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") edited TaskList(id: " + listToAdd.Id + ", name:" + listToAdd.Name + ")"),
                    EventType = EventTypes.TasksListCreated
                });
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
                    _context.eventLogs.Add(new EventLog()
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
                .FirstOrDefaultAsync(m => m.Id == id);


            if (list == null)
            {
                return NotFound();
            }
            if (list.Aim.ApplicationUserId != user.Id)
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
            var list = await _context.TaskList.Include(l => l.UserTasks).SingleAsync(l => l.Id == id);
            _context.TaskList.Remove(list);
            _context.eventLogs.Add(new EventLog()
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
         public async void ChangeTaskListExpand(int id, bool IsExpanded)
        {
            var taskListSettings = _context.aimSettings.First(s => s.AimId == id);
            taskListSettings.Expanded = IsExpanded;
            _context.Update(taskListSettings);
            await _context.SaveChangesAsync();
        }
      
        public TaskListSettings TaskListSettingsIsExistOrCreate(int listId)
        {
            if (!_context.taskListSettings.Any(s => s.TaskListId == listId))
            {
                TaskListSettings listSettings = new TaskListSettings()
                {
                    TaskListId= listId,
                    Expanded = false,
                    TaskList = _context.TaskList.First(s => s.Id == listId)

                };
                _context.taskListSettings.Add(listSettings);
                _context.SaveChanges();
                return listSettings;
            }
            else return _context.taskListSettings.First(s => s.TaskListId == listId);
        }


    }
}
