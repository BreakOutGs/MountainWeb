using MountainWeb.Data.Entities;
using System.Collections.Generic;

namespace MountainWeb.Models.AdminPanelViewModels
{
    public class UserProfileAdditionalViewModel
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }

        public ICollection<Aim> Aims { get; set; }
    }
}
