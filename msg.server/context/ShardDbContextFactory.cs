using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace msg.server {
    public class ShardDbContextFactory : IDesignTimeDbContextFactory<MSGContext> {


        public MSGContext CreateDbContext(string[] args) {
            return CreateDbContext("Server=51.15.106.177;Database=msgD2;User Id=sa;Password=yourStrong(!)Password;");
        }
        
        public MSGContext CreateDbContext(string connectionString) {
            var optsBldr = new DbContextOptionsBuilder<MSGContext>();
            optsBldr.UseSqlServer(connectionString);
            var newContext = new MSGContext(optsBldr.Options);
            return newContext;
        }
    }
}