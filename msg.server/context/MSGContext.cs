using Microsoft.EntityFrameworkCore;
using msg.lib;

namespace msg.server {
    public class MSGContext : DbContext {
        public MSGContext(DbContextOptions<MSGContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialogue> Dialogues { get; set; }


    }
}