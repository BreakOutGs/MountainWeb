using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AppUser.Edit
{
    public class EditAvatarVM
    {
      
        public EditAvatarVM() { AvatarPath = null; }
        public string AvatarPath { get; set; }
    }
}
