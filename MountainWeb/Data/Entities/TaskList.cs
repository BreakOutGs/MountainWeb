using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class TaskList
    {
        public TaskList()
        {
            UserTasks = new List<UserTask>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }

        public int AimId { get; set; }

        public Aim Aim { get; set; }
    }
}
