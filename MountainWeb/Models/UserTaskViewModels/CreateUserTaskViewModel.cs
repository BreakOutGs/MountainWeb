using System;
using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.UserTaskViewModels
{

    public class CreateUserTaskViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 100)]
        public int Priority { get; set; }

    }
}
