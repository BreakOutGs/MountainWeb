using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class Workspace
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Aim> Aims { get; set; }
        public WorkspaceSettings Settings {get;set;}

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public Workspace()
        {
            Aims = new List<Aim>();
            Settings = new WorkspaceSettings();
        }

    }
}
