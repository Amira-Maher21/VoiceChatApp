
using VoiceChatApp.Data;
using VoiceChatApp.Models;

namespace VoiceChatApp.Service
{
    public class CallLogService
    {
        private readonly AppDbContext _context;

        public CallLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> StartCallAsync(string caller, string receiver, string room)
        {
            var log = new CallLog
            {
                Caller = caller,
                Receiver = receiver,
                Room = room,
                StartTime = DateTime.UtcNow
            };

            _context.CallLogs.Add(log);
            await _context.SaveChangesAsync();

            return log.Id;
        }

        public async Task EndCallAsync(int callId)
        {
            var log = await _context.CallLogs.FindAsync(callId);
            if (log != null)
            {
                log.EndTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}






