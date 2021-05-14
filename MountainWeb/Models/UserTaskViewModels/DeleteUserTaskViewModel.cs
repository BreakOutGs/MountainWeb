using System;
using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class DeleteUserTaskViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0, 100)]
        public int Priority { get; set; }
    }
}
