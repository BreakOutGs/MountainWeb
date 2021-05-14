namespace MountainWeb.Data.Entities
{
    public class ListEntitySettings
    {
        public int Id { get; set; }

        public bool Expanded { get; set; }

        public ListEntitySettings()
        {
            Expanded = false;
        }

    }
}
