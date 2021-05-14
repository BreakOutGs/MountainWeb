using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MountainWeb.Data.Entities
{
    public class Aim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }


        public AimSettings Settings { get; set; }

        public Aim()
        {
            TaskLists = new List<TaskList>();
        }
    }
}
