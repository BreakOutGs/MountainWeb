using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.WorkspaceGroup
{
    public class DeleteVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DeleteVM(Workspace workspace)
        {
            this.Id = workspace.Id;
            this.Name = workspace.Name;
        }
    }
}
