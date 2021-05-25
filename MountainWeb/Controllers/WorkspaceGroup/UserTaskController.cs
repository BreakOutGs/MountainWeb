using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.UserTaskViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Controllers.WorkspaceGroup
{
    public class UserTaskController : Controller
    {

        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserTaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateUserTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CreateUserTaskViewModel model = new CreateUserTaskViewModel();
            return View("CreateUserTask", model);
        }

        // POST: Workspace/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserTask(int id, CreateUserTaskViewModel createViewModel)
        {

            if (ModelState.IsValid)
            {
                var list = await _context.TaskList.Include(l => l.Aim).
                    ThenInclude(a => a.Workspace).
                    SingleAsync(l => l.Id == id);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.Id != list.Aim.Workspace.ApplicationUserId) return NotFound();
                UserTask userTask = new UserTask()
                {
                    Name = createViewModel.Name,
                    Priority = createViewModel.Priority,
                    Description = createViewModel.Description,
                    TaskListId = list.Id,
                    TaskList = list
                };


                _context.UserTask.Add(userTask);
                _context.EventLogs.Add(new EventLog()
                {
                    Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") created UserTask(id: " + userTask.Id + ", name:" + userTask.Name + ")"),
                    EventType = EventTypes.TaskCreated
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Workspace", "");
            }
            return View(createViewModel);
        }

        // GET: Workspace/Edit/5
        public async Task<IActionResult> EditUserTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userTask = await _context.UserTask.Include(l => l.TaskList).
                ThenInclude(l => l.Aim).
                ThenInclude(a => a.Workspace).
                Include(ut => ut.Settings).
                Include(ut => ut.Reminds).
                SingleAsync(t => t.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }
            EditUserTaskViewModel editViewModel = new EditUserTaskViewModel()
            {
                Id = userTask.Id,
                Priority = userTask.Priority,
                Name = userTask.Name,
                Description = userTask.Description,
                IsCompleted = userTask.IsCompleted,
                Settings = userTask.Settings,
                Reminds = userTask.Reminds,
                AppUserId = userTask.TaskList.Aim.Workspace.ApplicationUserId
            };

            if (user.Id != userTask.TaskList.Aim.Workspace.ApplicationUserId)
            {
                return NotFound();
            }
            return View("UserTaskEditUpdated", editViewModel);
        }

        // POST: Workspace/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserTask(int id, EditUserTaskViewModel editModel)
        {
            if (id != editModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userTask = await _context.UserTask.SingleAsync(e => e.Id == editModel.Id);
                {//changing aimModel props by new values from modelview
                    userTask.Name = editModel.Name;
                    userTask.Priority = editModel.Priority;
                    userTask.Description = editModel.Description;
                    userTask.IsCompleted = editModel.IsCompleted;
                    userTask.Settings = editModel.Settings;
                    userTask.Reminds = editModel.Reminds;
                }

                try
                {
                    _context.Update(userTask);
                    _context.EventLogs.Add(new EventLog()
                    {
                        Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") edited UserTask(id: " + userTask.Id + ", name:" + userTask.Name + ")"),
                        EventType = EventTypes.TaskEdited
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExist(userTask.Id))
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
            return View(editModel);
        }

        // GET: Workspace/Delete/5
        public async Task<IActionResult> DeleteUserTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userTask = await _context.UserTask
                .Include(t => t.TaskList)
                .ThenInclude(l => l.Aim)
                .ThenInclude(a => a.Workspace)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (userTask == null)
            {
                return NotFound();
            }
            if (userTask.TaskList.Aim.Workspace.ApplicationUserId != user.Id)
            {
                return NotFound();
            }

            var deleteViewModel = new DeleteUserTaskViewModel()
            {
                Name = userTask.Name,
                Priority = userTask.Priority,
                Id = userTask.Id
            };

            return View("DeleteUserTask", deleteViewModel);
        }

        // POST: Workspace/Delete/5
        [HttpPost, ActionName("DeleteUserTask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserTaskConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userTask = await _context.UserTask.FindAsync(id);
            _context.UserTask.Remove(userTask);
            _context.EventLogs.Add(new EventLog()
            {
                Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") deleted UserTask(id: " + userTask.Id + ", name:" + userTask.Name + ")"),
                EventType = EventTypes.TaskRemoved
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Workspace", "");
        }


        private bool UserTaskExist(int id)
        {
            return _context.UserTask.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> ChangeTaskCompleted(int id)
        {
            if (!UserTaskExist(id)) return false;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userTask = await _context.UserTask.Include(t => t.TaskList).ThenInclude(l => l.Aim).SingleAsync(t => t.Id == id);
            if (user.Id != userTask.TaskList.Aim.Workspace.ApplicationUserId) return false;
            userTask.IsCompleted = !userTask.IsCompleted;
            try
            {
                _context.Update(userTask);
                _context.EventLogs.Add(new EventLog()
                {
                    Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") changes completing UserTask(id: " + userTask.Id + ", name:" + userTask.Name + ")"),
                    EventType = EventTypes.TaskEdited
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTaskExist(userTask.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }
        public bool AddRemind(int TaskId, DateTime dateTime, int IntervalMinutes)
        {
            var task = _context.UserTask.Single(t => t.Id == TaskId);
            Remind remind = new Remind()
            {
                DateTime = dateTime,
                MinuteInterval = IntervalMinutes,
                Task = task,
                TaskId = TaskId
            };
            task.Reminds.Add(remind);
            _context.Reminds.Add(remind);
            _context.SaveChanges();
            return true;
        }

    }
}
