﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.UserTaskViewModels
{
   
    public class CreateUserTaskViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }



    }
}
