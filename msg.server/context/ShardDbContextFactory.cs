using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace msg.server {
    public class ShardDbContextFactory : IDesignTimeDbContextFactory<MSGContext> {


        public MSGContext CreateDbContext(string[] args) {
            return CreateDbContext("Data Source=.;Initial Catalog=msgD2;Integrated Security=True;");
        }
        
        public MSGContext CreateDbContext(string connectionString) {
            var optsBldr = new DbContextOptionsBuilder<MSGContext>();
            optsBldr.UseSqlServer(connectionString);
            var newContext = new MSGContext(optsBldr.Options);
            return newContext;
        }
    }
}