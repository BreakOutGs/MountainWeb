using MountainWeb.Data.Entities;
using MountainWeb.Models.TaskListViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AimViewModels
{
    public class ShowAimViewModel
    {


            public readonly int Id;
            public string Name { get; set; }
            public int SummaryPriority { get; private set; }
            public int AveragePriority { get; private set; }

        public ICollection<ShowTaskListViewModel> TaskLists { get; set; }

            int completingAll;
            public string Completing { get; private set; }

            public ShowAimViewModel()
            {
                TaskLists = new List<ShowTaskListViewModel>();
                SummaryPriority = 0;
                AveragePriority = 0;
            }

            public ShowAimViewModel(Aim aim)
            {
                this.Id = aim.Id;
                this.Name = aim.Name;
                SummaryPriority = 0;
                AveragePriority = 0;
                 completingAll = 0;
                TaskLists = new List<ShowTaskListViewModel>();
                foreach (var list in aim.TaskLists)
                {
                     var _list = new ShowTaskListViewModel(list);
                    SummaryPriority += _list.SummaryPriority;
                    TaskLists.Add(_list);
                     completingAll += _list.getCompletingPercent();
                }
                if(TaskLists.Count!=0)
             AveragePriority = SummaryPriority / TaskLists.Count;
            if (TaskLists.Count > 0)
                Completing = completingAll / TaskLists.Count + "%";
            else Completing = "100%";

            }
    }
}
