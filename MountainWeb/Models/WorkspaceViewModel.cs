using MountainWeb.Data.Entities;
using MountainWeb.Models.AimViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models
{

    public class WorkspaceViewModel
    {

        public Dictionary<bool, string> listExpandedStyle { get; private set; } = new Dictionary<bool, string> { { false, "Mactivedrop" }, { true, "" } };
      
        
       

        public ICollection<ShowAimViewModel> Aims { get; set; }

        public WorkspaceViewModel(ICollection<Aim> aims)
        {
            Aims = new List<ShowAimViewModel>();
            foreach(var aim in aims)
            {
                Aims.Add(new ShowAimViewModel(aim));
            }
        }
    }
}
