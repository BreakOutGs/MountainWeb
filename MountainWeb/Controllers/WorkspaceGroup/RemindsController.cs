using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Controllers.WorkspaceGroup
{
    public class RemindsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public RemindsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        [HttpPost]
        public int CreateRemind(int TaskId, DateTime dateTime, int minuteinterval)
        {
            var task = _context.UserTask.Include(task => task.Reminds).Single(t => t.Id == TaskId);

            Remind remind = new Remind()
            {
                Task = task,
                TaskId = TaskId,
                DateTime = dateTime,
                MinuteInterval = minuteinterval,
                TaskName = task.Name
            };
            task.Reminds.Add(remind);
            _context.Reminds.Add(remind);
            _context.UserTask.Update(task);
            _context.SaveChanges();
            return remind.Id;
        }

        [HttpPost]
        public bool ChangeRemind(int RemindId, DateTime dateTime, int minuteinterval)
        {
            Remind remind = _context.Reminds.Single(r => r.Id == RemindId);
            remind.DateTime = dateTime;
            remind.MinuteInterval = minuteinterval;
            _context.Reminds.Update(remind);
            _context.SaveChanges();
            return true;
        }

        [HttpPost]
        public bool DeleteRemind(int RemindId)
        {
            Remind remind = _context.Reminds.Single(r => r.Id == RemindId);
            _context.Reminds.Remove(remind);
            _context.SaveChanges();
            return true;
        }
        

        [HttpPost]
        public  JsonResult GetReminds()
        {
            var userId = _userManager.GetUserId(User);
            var l = _context.Reminds.Where(r => r.Task.TaskList.Aim.Workspace.ApplicationUserId == userId)
                .ToList().OrderBy(rem => rem.DateTime);

            var data2send = Json(l);
            return data2send;
        }

        public IActionResult ShowUserTaskEditByRemind(int id)
        {
            var rem = _context.Reminds
                .Single(r => r.Id == id);
            return RedirectToAction("EditUserTask", "UserTask", new {id = rem.TaskId });
        }


    }
}
