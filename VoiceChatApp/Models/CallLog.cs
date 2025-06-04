namespace VoiceChatApp.Models
{
    public class CallLog
    {
        public int Id { get; set; }
        public string Caller { get; set; }
        public string Receiver { get; set; }
        public string Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
