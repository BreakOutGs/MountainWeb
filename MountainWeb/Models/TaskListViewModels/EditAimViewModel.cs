using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.TaskListViewModels
{
    public class EditTaskListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AppUseId { get; set; }
    }
}
