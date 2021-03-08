using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Data.Entities
{

   public enum EventTypes
    {
        UserRegistered,
        AimCreated,
        AimEdited,
        AimRemoved,
        TaskCreated,
        TaskEdited,
        TaskRemoved,
        TasksListCreated,
        TasksListEdited,
        TasksListRemoved
    }

    public class EventLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public EventTypes EventType { get; set; }
    }
}
