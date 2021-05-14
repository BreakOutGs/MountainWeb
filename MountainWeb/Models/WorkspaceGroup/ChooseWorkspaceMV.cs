using MountainWeb.Data.Entities;
using System.Collections.Generic;

namespace MountainWeb.Models.WorkspaceGroup
{
    public class ChooseWorkspaceMV
    {
        public List<Workspace> Workspaces { get; set; }

        public int GetTaskListsCount(Workspace workspace)
        {
            int count = 0;
            foreach (Aim aim in workspace.Aims)
            {
                foreach (TaskList list in aim.TaskLists)
                {
                    count++;
                }
            }
            return count;
        }
        public int GetTasksCount(Workspace workspace)
        {
            int count = 0;
            foreach (Aim aim in workspace.Aims)
            {
                foreach (TaskList list in aim.TaskLists)
                {
                    foreach (UserTask task in list.UserTasks)
                        count++;
                }
            }
            return count;
        }

    }
}
