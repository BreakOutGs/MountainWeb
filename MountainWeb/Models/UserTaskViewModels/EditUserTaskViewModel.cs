using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class EditUserTaskViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
        [Range(0, 100)]
        public int Priority { get; set; }

        public string AppUserId { get; set; }

        public List<Remind> Reminds { get; set; }
        public UserTaskSettings Settings { get; set; }
    }
}
