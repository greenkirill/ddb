using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using msg.lib;

namespace msg.client {
    public class Client : IDisposable {
        public class StateObject {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        public Client(string hostname, int port) {
            IpHostInfo = Dns.GetHostEntry(hostname);
            Port = port;
            client = new TcpClient(hostname, port);
            // client = new Socket(IpAddress.AddressFamily,
            //     SocketType.Stream, ProtocolType.Tcp);
            bh = new BlockHelper(client);
        }


        public delegate void ProfileEvent(Profile Profile);
        public event ProfileEvent ProfileRecieved;

        private BlockHelper bh;

        public string Hostname { get; }
        public int Port { get; }
        private TcpClient client { get; }
        private NetworkStream _stream;
        public IPHostEntry IpHostInfo { get; }
        public IPAddress IpAddress {
            get {
                return IpAddresses[0];
            }
        }
        private IPAddress[] IpAddresses {
            get {
                return Array.FindAll(
                        IpHostInfo.AddressList,
                        a => a.AddressFamily == AddressFamily.InterNetwork);
            }
        }

        private Task recieveTask { get; set; }

        public void Start() {
            client.Connect(IpAddress, Port);

            _stream = client.GetStream();

            recieveTask = new Task(Recieve);
            recieveTask.Start();
        }

        public void Dispose() {
            ((IDisposable)client).Dispose();
        }

        public void Registration(string username, string password) {
            var rb = new RegisterBlock(username, password);
            bh.Send(rb);
        }
        public void Auth(string username, string password) {
            var rb = new AuthBlock(username, password);
            bh.Send(rb);
        }

        ManualResetEvent ShutdownEvent = new ManualResetEvent(true);
        public void Recieve() {
            // try {
            //     // ShutdownEvent is a ManualResetEvent signaled by
            //     // Client when its time to close the socket.
            //     while (!ShutdownEvent.WaitOne(0)) {
            //         try {
            //             if (!_stream.DataAvailable) {
            //                 Thread.Sleep(1);
            //             } else if (_stream.Read(_data, 0, _data.Length) > 0) {

            //                 var header = bh.RecieveHead();

            //             } else {
            //                 ShutdownEvent.Set();
            //             }
            //         } catch (IOException ex) {
            //             // Handle the exception...
            //         }
            //     }
            // } catch (Exception ex) {
            //     // Handle the exception...
            // } finally {
            //     _stream.Close();
            // }

            var header = bh.RecieveHead();
            // bool tokenRecieved = false;
            while ((header.type != 0 || header.size != 0) && header.type != BlockTypeConstants.ENDConnect) {
                switch (header.type) {
                    case BlockTypeConstants.ErrorBlock:
                        break;
                    case BlockTypeConstants.ProfileBlock:
                        Console.WriteLine("123");
                        RecieveProfile(header.size);
                        break;
                    default:
                        return;
                }
                header = bh.RecieveHead();
            }
        }

        public byte[] Recieve(int size) {
            var sb = new StringBuilder();
            var buffer = new byte[size];
            if (_stream.Read(buffer, 0, size) != size)
                return null;
            return buffer;
        }

        public void RecieveProfile(int size) {
            var tBlock = bh.RecieveBlock(size, new ProfileBlock());
            ProfileRecieved.Invoke(tBlock.Profile);
        }
    }

}
