using MountainWeb.Data.Entities;

namespace MountainWeb.Models.EventLogViewModels
{
    public class ShowEventLogViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public EventTypes EventType { get; set; }
        public ShowEventLogViewModel(EventLog eventLog)
        {
            this.Id = eventLog.Id;
            this.Message = eventLog.Message;
            this.EventType = eventLog.EventType;
        }
    }
}
