using System;

namespace MountainWeb.Data.Entities
{
    enum TimeType : int
    {
        Minute = 1,
        Hour = 60,
        Day = 1440,
        Week = 10080
    }

    public class Remind
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int MinuteInterval { get; set; }
        bool isRepeatable;

        public UserTask Task { get; set; }
        public int TaskId { get; set; }




    }
}
