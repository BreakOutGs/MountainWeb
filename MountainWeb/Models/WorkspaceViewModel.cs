using MountainWeb.Data.Entities;
using MountainWeb.Models.AimViewModels;
using System.Collections.Generic;

namespace MountainWeb.Models
{

    public class WorkspaceViewModel
    {

        public Dictionary<bool, string> listExpandedStyle { get; private set; } = new Dictionary<bool, string> { { false, "Mactivedrop" }, { true, "" } };

        public int WorkspaceId { get; set; }
        public string Name { get; set; }


        public ICollection<ShowAimViewModel> Aims { get; set; }

        public WorkspaceViewModel(Workspace workspace)
        {
            if (workspace != null)
            {
                WorkspaceId = workspace.Id;
                var aims = workspace.Aims;
                Aims = new List<ShowAimViewModel>();
                Name = workspace.Name;
                foreach (var aim in aims)
                {
                    Aims.Add(new ShowAimViewModel(aim));
                }
            }
            else
            {
                Aims = new List<ShowAimViewModel>();
            }


        }
        public  string getExpandedIcon(bool isExpanded)
        {
            if (isExpanded)
            {
                return "fas fa-caret-down";
            }
            else return "fas fa-caret-left";
        }
        public  string getStyleForContainer(bool isExpanded)
        {
            if (isExpanded)
            {
                return "display:block;";
            }

                return "display:none;";
           
        }
        public string getCheckedByBool(bool B)
        {
            if (B)
            {
                return "checked";
            }
            else return "";
        }
    }
}
