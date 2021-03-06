using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace msg.server {
    public class Server {
        public Server(int port, IMessenger messenger) {
            Port = port;

            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            Messenger = messenger;
        }

        public int Port { get; }

        private Socket listener;
        private IMessenger Messenger;
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public void listen() {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, Port);
            try {
                listener.Bind(localEP);
                listener.Listen(100);

                while (true) {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    allDone.WaitOne();
                }

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
        public void AcceptCallback(IAsyncResult ar) {
            try {
                allDone.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                Messenger.CreateNewSession(handler);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                Console.WriteLine("CLSOED");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}