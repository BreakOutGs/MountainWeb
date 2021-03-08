using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class UserTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted{ get; set; }




        public int TaskListId { get; set; }

        public TaskList TaskList { get; set; }
    }
}
