using System.ComponentModel.DataAnnotations;

namespace MountainWeb.Models.AimViewModels
{
    public class CreateAimViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int WorkspaceId { get; set; }

    }
}
