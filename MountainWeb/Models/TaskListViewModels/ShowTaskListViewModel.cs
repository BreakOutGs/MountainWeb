using MountainWeb.Data.Entities;
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

            public int SummaryPriority { get; private set; }
            public int AveragePriority { get; private set; }

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
                SummaryPriority = 0;
                AveragePriority = 0;
                UserTasks = new List<ShowUserTaskViewModel>();
                foreach (var task in list.UserTasks)
                {
                   SummaryPriority += task.Priority;
                    UserTasks.Add(new ShowUserTaskViewModel(task));
                   if (task.IsCompleted) taskCompleted++;
                }
                if(UserTasks.Count!=0)
                AveragePriority = SummaryPriority / UserTasks.Count;
                Completing = taskCompleted + "/" + UserTasks.Count;
            }
            public int getCompletingPercent()
            {
            if (UserTasks.Count > 1)
                return (taskCompleted * 100) / UserTasks.Count;
            else return 100;
             }
        
    }
}
