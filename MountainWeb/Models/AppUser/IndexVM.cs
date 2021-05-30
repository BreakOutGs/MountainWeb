using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AppUser
{
    public class IndexVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public bool EmailConfirmed{ get; set; }
        public IndexVM() { Id = "0"; Name = "uknown"; Email = "empty"; AvatarPath = ""; }
        public IndexVM(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.Email;
            AvatarPath = user.AvatarPath;
            EmailConfirmed = user.EmailConfirmed;
        }
    }
}
