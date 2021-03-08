using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class ShowUserTaskViewModel
    {
            public readonly int Id;
            public string Name { get; set; }
            public bool IsCompleted { get; set; }
        [Range(0, 1)]
        public double Priority { get; set; }

        public ShowUserTaskViewModel(UserTask userTask)
            {
                this.Id = userTask.Id;
                this.Priority = userTask.Priority/100.0;
                this.Name = userTask.Name;
                this.IsCompleted = userTask.IsCompleted;
            }
        
    }
}
