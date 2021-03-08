using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MountainWeb.Data.Entities;
using MountainWeb.Models;
using MountainWeb.Models.UserTaskViewModels;

namespace MountainWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        public DbSet<MountainWeb.Data.Entities.Aim> Aim { get; set; }
        public DbSet<MountainWeb.Data.Entities.TaskList> TaskList  { get; set; }
        public DbSet<MountainWeb.Data.Entities.UserTask> UserTask { get; set; }
       // public DbSet<MountainWeb.Models.UserTaskViewModels.EditUserTaskViewModel> EditUserTaskViewModel { get; set; }
        public DbSet<MountainWeb.Data.Entities.EventLog> eventLogs { get; set; }
        
    }
}
