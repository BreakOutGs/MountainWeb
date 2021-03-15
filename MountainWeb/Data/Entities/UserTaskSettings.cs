using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class UserTaskSettings 
    {
        public int Id { get; set; }
        
        public int UserTaskId { get; set; }
        public UserTask UserTask { get; set; }
    }
}
