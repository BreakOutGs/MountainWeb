using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel;
namespace MountainWeb.Data.Entities
{
    public class Aim
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        public string Name { get; set; }
       
        public string Description { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }

        public AimSettings Settings { get; set; }

        public Aim() {
            TaskLists = new List<TaskList>();
        }
    }
}
