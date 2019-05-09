using System;

namespace msg.server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(2121);
            server.listen();
        }
    }
}
