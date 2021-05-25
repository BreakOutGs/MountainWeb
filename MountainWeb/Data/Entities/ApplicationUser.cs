﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MountainWeb.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Workspaces = new List<Workspace>();
        }
        public ICollection<Workspace> Workspaces { get; set; }
        public int CurrentWorkspaceId { get; set; }
    }
}
