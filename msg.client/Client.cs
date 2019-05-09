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
        }

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
            Send(rb);
        }
        public void Auth(string username, string password) {
            var rb = new AuthBlock(username, password);
            Send(rb);
        }

        public void Send(IBlock filePiece, bool wait = true) {
            client.Send(filePiece.GetBytes());
        }

        public async Task Recieve() {
            var header = RecieveHead();
            // bool tokenRecieved = false;
            while ((header.type != 0 || header.size != 0) && header.type != BlockTypeConstants.ENDConnect) {
                switch (header.type) {
                    case BlockTypeConstants.ErrorBlock:
                        break;
                    default:
                        return;
                }
                header = RecieveHead();
            }
        }

        public byte[] Recieve(int size) {
            var sb = new StringBuilder();
            var buffer = new byte[size];
            if (client.Receive(buffer, 0, size, 0) != size)
                return null;
            return buffer;
        }
        private (byte type, int size) RecieveHead() {
            var idBuffer = new byte[1];
            var idL = client.Receive(idBuffer);
            if (idL <= 0)
                throw new Exception("expected id (1 byte)");
            var sizeBuffer = new byte[4];
            var sizeL = client.Receive(sizeBuffer);
            if (sizeL <= 0)
                throw new Exception("expected piece size (8 bytes)");
            return (idBuffer[0], BitConverter.ToInt32(sizeBuffer, 0));
        }

        public void RecieveToken(int size) {
            var tBlock = RecieveBlock(size, new TokenBlock());
            TokenRecived.Invoke(tBlock.Token);
        }

        public X RecieveBlock<X>(int size, X lBlock) where X : IBlock {
            lBlock.SetSize(size);
            var buffer = new byte[size];
            var l = client.Receive(buffer);
            if (l <= 0)
                throw new Exception($"expected fileSizePiece ({size} bytes)");
            lBlock.SetBody(buffer);
            return lBlock;
        }
    }

}
