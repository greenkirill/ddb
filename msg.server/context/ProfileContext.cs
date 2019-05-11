using Microsoft.EntityFrameworkCore;
using msg.lib;

namespace msg.server {
    public class ProfileContext : DbContext {

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=51.15.106.177;Database=msgs;User Id=sa;Password=yourStrong(!)Password;");
        }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Profile>(entity => {
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}