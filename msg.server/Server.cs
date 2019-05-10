using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace msg.server {
    public class Server {
        public Server(int port, IMessenger messenger) {
            Port = port;
            listener2 = new TcpListener(IPAddress.Any, Port);
            // listener = new Socket(AddressFamily.InterNetwork,
            //     SocketType.Stream, ProtocolType.Tcp);
            Messenger = messenger;
        }

        public int Port { get; }

        private TcpListener listener2;
        private Socket listener;
        private IMessenger Messenger;
        public ManualResetEvent allDone = new ManualResetEvent(false);
        // public void listen() {
        //     IPEndPoint localEP = new IPEndPoint(IPAddress.Any, Port);
        //     try {
        //         listener.Bind(localEP);
        //         listener.Listen(100);

        //         while (true) {
        //             allDone.Reset();

        //             Console.WriteLine("Waiting for a connection...");
        //             listener.BeginAccept(
        //                 new AsyncCallback(AcceptCallback),
        //                 listener);
                
        //             allDone.WaitOne();
        //         }

        //     } catch (Exception e) {
        //         Console.WriteLine(e.ToString());
        //     }

        //     Console.WriteLine("\nPress ENTER to continue...");
        //     Console.Read();
        // }
        public void listen() {
            try {
                listener2.Start();

                while (true) {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener2.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener2);
                    // listener2
                    // listener.BeginAccept(
                    //     new AsyncCallback(AcceptCallback),
                    //     listener);

                    allDone.WaitOne();
                }

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
        // public void AcceptCallback(IAsyncResult ar) {
        //     try {
        //         allDone.Set();

        //         Socket listener = (Socket)ar.AsyncState;
        //         Socket handler = listener.EndAccept(ar);
                
        //         Messenger.CreateNewSession(handler);

        //         handler.Shutdown(SocketShutdown.Both);
        //         handler.Close();
        //     } catch (Exception e) {
        //         Console.WriteLine(e.ToString());
        //     }
        // }
        public void AcceptCallback(IAsyncResult ar) {
            try {
                allDone.Set();

                TcpListener listener = (TcpListener) ar.AsyncState;
                TcpClient handler = listener.EndAcceptTcpClient(ar);

                // Socket listener = (Socket)ar.AsyncState;
                // Socket handler = listener.EndAccept(ar);
                
                Messenger.CreateNewSession(handler);
                handler.Close();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}