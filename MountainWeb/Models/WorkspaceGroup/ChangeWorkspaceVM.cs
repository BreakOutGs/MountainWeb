using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.WorkspaceGroup
{
    public class ChangeWorkspaceVM
    {
        public int Id {  get; private set; }
        public string Name { get; set; }
        public ChangeWorkspaceVM(Workspace workspace)
        {
            Id = workspace.Id;
            Name = workspace.Name;
        }
        public ChangeWorkspaceVM()
        {

        }
    }
}
