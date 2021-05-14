using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.TaskListViewModels
{
    public class CreateTaskListViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }


    }
}
