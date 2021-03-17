using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{
    public class AimSettings : ListEntitySettings
    {

        public int AimId { get; set; }
        public Aim Aim{ get; set; }

      
    }
}
