using MountainWeb.Data.Entities;

namespace MountainWeb.Models.WorkspaceGroup
{
    public class ChangeWorkspaceVM
    {
        public int Id { get; private set; }
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
