namespace MountainWeb.Data.Entities
{
    public class TaskListSettings : ListEntitySettings
    {

        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; }


    }
}
