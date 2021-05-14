using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data;
using MountainWeb.Data.Entities;
using MountainWeb.Models.AimViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Controllers.WorkspaceGroup
{
    public class AimController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AimController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        // GET: Workspace/Create
        public IActionResult CreateAim(int id)
        {
            CreateAimViewModel model = new CreateAimViewModel();
            {
                model.WorkspaceId = id;
            }
            return View("CreateAim", model);
        }

        // POST: Workspace/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAim(CreateAimViewModel createAimViewModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Aim aimToAdd = new Aim()
                {
                    Name = createAimViewModel.Name,
                    Description = createAimViewModel.Description,
                    WorkspaceId = createAimViewModel.WorkspaceId
                };

                _context.Aim.Add(aimToAdd);

                _context.EventLogs.Add(new EventLog()
                {
                    Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") created Aim(id: " + aimToAdd.Id + ", name:" + aimToAdd.Name + ")"),
                    EventType = EventTypes.AimCreated
                }
                );
                await _context.SaveChangesAsync();

                aimToAdd.Settings = new AimSettings()
                {
                    Aim = aimToAdd,
                    AimId = aimToAdd.Id
                };
                _context.AimSettings.Add(aimToAdd.Settings);
                _context.Aim.Update(aimToAdd);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Workspace", "");
            }
            return View(createAimViewModel);
        }

        // GET: Workspace/Edit/5
        public async Task<IActionResult> EditAim(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var aim = await _context.Aim.FindAsync(id);
            EditAimViewModel editAimViewModel = new EditAimViewModel()
            {
                Id = aim.Id,
                Name = aim.Name,
                Description = aim.Description
            };
            if (aim == null)
            {
                return NotFound();
            }
            if (user.Id != aim.Workspace.ApplicationUserId)
            {
                return NotFound();
            }
            return View("EditAim", editAimViewModel);
        }

        // POST: Workspace/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAim(int id, EditAimViewModel editModel)
        {
            if (id != editModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var aim = await _context.Aim.SingleAsync(e => e.Id == editModel.Id);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                {//changing aimModel props by new values from modelview
                    aim.Name = editModel.Name;
                    aim.Description = editModel.Description;
                }

                try
                {
                    _context.Update(aim);
                    _context.EventLogs.Add(new EventLog()
                    {
                        Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") edited Aim(id: " + aim.Id + ", name:" + aim.Name + ")"),
                        EventType = EventTypes.AimEdited
                    });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AimExists(aim.Id))
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
        public async Task<IActionResult> DeleteAim(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var aim = await _context.Aim
                .FirstOrDefaultAsync(m => m.Id == id);


            if (aim == null)
            {
                return NotFound();
            }
            if (aim.Workspace.ApplicationUserId != user.Id)
            {
                return NotFound();
            }

            var deleteAimViewModel = new DeleteAimViewModel()
            {
                Name = aim.Name,
                Id = aim.Id
            };

            return View("DeleteAim", deleteAimViewModel);
        }

        // POST: Workspace/Delete/5
        [HttpPost, ActionName("DeleteAim")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAimConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var aim = await _context.Aim
                .Include(a => a.Settings)
                .Include(aim => aim.TaskLists)
                .ThenInclude(e => e.UserTasks)
                .Include(aim => aim.TaskLists)
                .ThenInclude(l => l.Settings)
                .SingleAsync(a => a.Id == id);
            _context.EventLogs.Add(new EventLog()
            {
                Message = ("User(id: " + user.Id + ", login: " + user.UserName + ") removed Aim(id: " + aim.Id + ", name:" + aim.Name + ")"),
                EventType = EventTypes.AimRemoved
            });
            _context.AimSettings.Remove(aim.Settings);
            _context.Aim.Remove(aim);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Workspace", "");
        }

        private bool AimExists(int id)
        {
            return _context.Aim.Any(e => e.Id == id);
        }
        public void ChangeAimExpand(int id, bool IsExpanded)
        {

            var aimSettings = AimSettingsExist(id);
            aimSettings.Expanded = IsExpanded;
            _context.Update(aimSettings);
            _context.SaveChanges();
        }
        public AimSettings AimSettingsExist(int aimId)
        {
            if (!_context.AimSettings.Any(s => s.AimId == aimId))
            {
                var aim = _context.Aim.First(aim => aim.Id == aimId);
                aim.Settings = new AimSettings()
                {
                    Aim = aim,
                    AimId = aim.Id
                };
                _context.AimSettings.Add(aim.Settings);
                _context.Update(aim);
                _context.SaveChanges();
                return aim.Settings;
            }
            else return _context.AimSettings.First(s => s.AimId == aimId);
        }

    }
}
