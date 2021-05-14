using MountainWeb.Data.Entities;

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
