using MountainWeb.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class ShowUserTaskViewModel
    {
        public readonly int Id;
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        [Range(0, 100)]
        public int Priority { get; set; }

        public ShowUserTaskViewModel(UserTask userTask)
        {
            this.Id = userTask.Id;
            this.Priority = userTask.Priority;
            this.Name = userTask.Name;
            this.IsCompleted = userTask.IsCompleted;
        }

    }
}
