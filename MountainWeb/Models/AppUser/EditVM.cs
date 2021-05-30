using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AppUser
{
    public class EditVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string RetryPassword { get; set; }

        public EditVM() { Id = "0"; Name = "uknown"; Email = "empty"; AvatarPath = ""; }
            public EditVM(ApplicationUser user)
            {
                Id = user.Id;
                Name = user.UserName;
                Email = user.Email;
                AvatarPath = user.AvatarPath;
            }
        
    }
}
