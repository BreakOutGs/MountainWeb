﻿using MountainWeb.Data.Entities;
using MountainWeb.Models.UserTaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.TaskListViewModels
{
    public class ShowTaskListViewModel
    {

            public readonly int Id;
            public string Name { get; set; }

            int taskCompleted;
            public string Completing {  get; private set; }

            public ICollection<ShowUserTaskViewModel> UserTasks { get; set; }
            public ShowTaskListViewModel()
            {
                UserTasks = new List<ShowUserTaskViewModel>();
            }

            public ShowTaskListViewModel(TaskList list)
            {
                taskCompleted = 0;
                this.Id = list.Id;
                this.Name = list.Name;
                UserTasks = new List<ShowUserTaskViewModel>();
                foreach (var task in list.UserTasks)
                {
                    UserTasks.Add(new ShowUserTaskViewModel(task));
                if (task.IsCompleted) taskCompleted++;
                }
                Completing = taskCompleted + "/" + UserTasks.Count ;
            }
            public int getCompletingPercent()
            {
            if (UserTasks.Count > 1)
                return (taskCompleted * 100) / UserTasks.Count;
            else return 100;
             }
        
    }
}