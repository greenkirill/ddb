using System;
using System.Collections.Generic;

namespace msg.server {
    class Program {
        static void Main(string[] args) {
            var conStrings = new List<string> {
                "Data Source=.;Initial Catalog=msgD1;Integrated Security=True;",
                "Data Source=.;Initial Catalog=msgD2;Integrated Security=True;"
            };
            
            var server = new Server(2121, new Messenger(conStrings));
            server.listen();
        }
    }
}
