using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models.UserTaskViewModels
{
    public class DeleteUserTaskViewModel
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        [Range(0, 100)]
        public int Priority { get; set; }
    }
}
