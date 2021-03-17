using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class TaskListSettings : ListEntitySettings
    {

        public int TaskListId  { get; set; }
        public TaskList TaskList { get; set; }
        
       
    }
}
