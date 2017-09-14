using System;

namespace GS.ErrorLookupService.ServiceModel.Models
{
    [Serializable()]
    public class ErrorOutput
    {
        public ErrorOutput(string time, string process, string area, string category, string eventid, string level,
            string message)
        {
            Time = time;
            Process = process;
            Area = area;
            Category = category;
            EventId = eventid;
            Level = level;
            Message = message;
        }

        public string Time { get; set; }
        public string Process { get; set; }
        public string Area { get; set; }
        public string Category { get; set; }
        public string EventId { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}
