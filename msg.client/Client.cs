using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using msg.lib;

namespace msg.client {
    class Client : IDisposable {
        public class StateObject {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        public Client(string hostname, int port) {
            IpHostInfo = Dns.GetHostEntry(hostname);
            Port = port;
            client = new Socket(IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            bh = new BlockHelper(client);
        }

        private BlockHelper bh;

        public delegate string TokenEvent(Token token);
        public event TokenEvent TokenRecived;

        public string Hostname { get; }
        public int Port { get; }
        private Socket client { get; }
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
            IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);
            client.Connect(remoteEP);
            recieveTask = Recieve();
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


        public async Task Recieve() {
            var header = bh.RecieveHead();
            // bool tokenRecieved = false;
            while ((header.type != 0 || header.size != 0) && header.type != BlockTypeConstants.ENDConnect) {
                switch (header.type) {
                    case BlockTypeConstants.ErrorBlock:
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
            if (client.Receive(buffer, 0, size, 0) != size)
                return null;
            return buffer;
        }

        public void RecieveToken(int size) {
            var tBlock = bh.RecieveBlock(size, new TokenBlock());
            TokenRecived.Invoke(tBlock.Token);
        }
    }

}
