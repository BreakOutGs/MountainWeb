using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AppUser.Edit
{
    public class EditEmailVM
    {
        [Required]
        public string Email { get; set; }

       
    }
}
