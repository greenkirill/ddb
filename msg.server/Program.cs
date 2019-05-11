using System;
using System.Collections.Generic;

namespace msg.server {
    class Program {
        static void Main(string[] args) {
            // var conStrings = new List<string> {
            //     "Data Source=.;Initial Catalog=msgD1;Integrated Security=True;",
            //     "Data Source=.;Initial Catalog=msgD2;Integrated Security=True;"
            // };
            var conStrings = new List<string> {
                "Server=51.15.106.177;Database=msgD1;User Id=sa;Password=yourStrong(!)Password;",
                "Server=51.15.106.177;Database=msgD2;User Id=sa;Password=yourStrong(!)Password;"
            };
            
            var server = new Server(2121, new Messenger(conStrings));
            server.listen();
        }
    }
}
