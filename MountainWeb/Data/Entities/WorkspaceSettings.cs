using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Data.Entities
{
    public class WorkspaceSettings
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int CurrentAim { get; set; }
    }
}
