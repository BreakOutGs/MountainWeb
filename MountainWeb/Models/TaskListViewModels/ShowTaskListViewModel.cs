using MountainWeb.Data.Entities;
using MountainWeb.Models.UserTaskViewModels;
using System.Collections.Generic;

namespace MountainWeb.Models.TaskListViewModels
{
    public class ShowTaskListViewModel
    {

        public readonly int Id;
        public string Name { get; set; }

        public int SummaryPriority { get; private set; }
        public int AveragePriority { get; private set; }

        int taskCompleted;
        public string Completing { get; private set; }

        public ICollection<ShowUserTaskViewModel> UserTasks { get; set; }
        public TaskListSettings Settings { get; set; }
        public ShowTaskListViewModel()
        {
            UserTasks = new List<ShowUserTaskViewModel>();
        }

        public ShowTaskListViewModel(TaskList list)
        {
            this.taskCompleted = 0;
            this.Id = list.Id;
            this.Name = list.Name;
            this.SummaryPriority = 0;
            this.AveragePriority = 0;
            this.Settings = list.Settings;
            UserTasks = new List<ShowUserTaskViewModel>();
            foreach (var task in list.UserTasks)
            {
                this.SummaryPriority += task.Priority;
                this.UserTasks.Add(new ShowUserTaskViewModel(task));
                if (task.IsCompleted) this.taskCompleted++;
            }
            if (UserTasks.Count != 0)
                this.AveragePriority = SummaryPriority / UserTasks.Count;
            this.Completing = taskCompleted + "/" + UserTasks.Count;
        }
        public int getCompletingPercent()
        {
            if (UserTasks.Count > 1)
                return (taskCompleted * 100) / UserTasks.Count;
            else return 100;
        }

    }
}
