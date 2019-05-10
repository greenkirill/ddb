using System;
using msg.lib;

namespace msg.client {
    class Program {
        static void Main(string[] args) {
            var m = new Messenger("localhost", 2121);
            m.Start();

        }
    }
}
