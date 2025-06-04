using Microsoft.EntityFrameworkCore;
using VoiceChatApp.Models;

namespace VoiceChatApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CallLog> CallLogs { get; set; }
    }
}
