using MountainWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.AppUser.Edit
{
    public class EditPasswordVM
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторіть пароль")]
        [Compare("NewPassword", ErrorMessage ="Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
        
       

    }
}
