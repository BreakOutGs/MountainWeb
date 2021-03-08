using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class ShowUserTaskViewModel
    {
            public readonly int Id;
            public string Name { get; set; }
            public bool IsCompleted { get; set; }

            public ShowUserTaskViewModel(UserTask userTask)
            {
                this.Id = userTask.Id;
                this.Name = userTask.Name;
                this.IsCompleted = userTask.IsCompleted;
            }
        
    }
}
