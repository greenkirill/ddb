using Microsoft.EntityFrameworkCore;
using msg.lib;

namespace msg.server {
    public class ProfileContext : DbContext {

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=msgs;Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Profile>(entity => {
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}