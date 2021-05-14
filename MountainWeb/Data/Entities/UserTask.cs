using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Data.Entities
{
    public class UserTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        [Range(0, 100)]
        public int Priority { get; set; }

        public UserTaskSettings Settings { get; set; }


        public int TaskListId { get; set; }

        public TaskList TaskList { get; set; }

        public List<Remind> Reminds { get; set; }
    }
}
