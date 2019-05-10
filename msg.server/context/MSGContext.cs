using Microsoft.EntityFrameworkCore;
using msg.lib;

namespace msg.server {
    public class MSGContext : DbContext {
        public MSGContext(DbContextOptions<MSGContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialogue> Dialogues { get; set; }
        public DbSet<Member> Members { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder) {
        //     builder.Entity<Member>().HasKey(table => new {
        //         table.ID, table.
        //     });
        // }
    }
}