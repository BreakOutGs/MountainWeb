using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data.Entities;
using MountainWeb.Models;
using MountainWeb.Models.UserTaskViewModels;
using MountainWeb.Models.WorkspaceGroup;

namespace MountainWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MountainWeb.Data.Entities.Workspace> Workspaces { get; set; }
        public DbSet<MountainWeb.Data.Entities.WorkspaceSettings> WorkspaceSettings { get; set; }


        public DbSet<MountainWeb.Data.Entities.Aim> Aim { get; set; }
        public DbSet<MountainWeb.Data.Entities.AimSettings> AimSettings { get; set; }

        public DbSet<MountainWeb.Data.Entities.TaskList> TaskList  { get; set; }
        public DbSet<MountainWeb.Data.Entities.TaskListSettings> TaskListSettings { get; set; }

        public DbSet<MountainWeb.Data.Entities.UserTask> UserTask { get; set; }
        public DbSet<MountainWeb.Data.Entities.UserTaskSettings> UserTaskSettings { get; set; }

        public DbSet<MountainWeb.Data.Entities.EventLog> EventLogs { get; set; }

        public DbSet<MountainWeb.Models.WorkspaceGroup.ChangeWorkspaceVM> ChangeWorkspaceVM { get; set; }


        
    }
}
