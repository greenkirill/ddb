using System;
using System.Collections.Generic;
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
            client = new Socket(IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            bh = new BlockHelper(client);
        }


        public delegate void ProfileEvent(Profile Profile);
        public event ProfileEvent ProfileRecieved;
        public delegate void UserListEvent(List<Profile> Profiles);
        public event UserListEvent UserListRecieved;
        public delegate void DialogueListEvent(List<Dialogue> Dialogues);
        public event DialogueListEvent DialogueListRecieved;
        public delegate void MsgListEvent(List<Message> Messages);
        public event MsgListEvent MsgListRecieved;
        public delegate void MsgEvent(Message Message);
        public event MsgEvent MsgRecieved;

        private BlockHelper bh;

        public string Hostname { get; }
        public int Port { get; }
        private Socket client { get; set; }
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
            RecieveOneBlock();
        }
        public void RUserList() {
            var rb = new RequestUserListBlock();
            bh.Send(rb);
            RecieveOneBlock();
        }
        public void RMsgList(Guid id) {
            var rb = new RequestMsgList(id);
            bh.Send(rb);
            RecieveOneBlock();
        }

        public void SendMsg(Guid did, string text) {
            var rb = new CMsgBlock(did, text);
            bh.Send(rb);
        }
        public void RDialogueList() {
            var rb = new RequestDialogueList();
            bh.Send(rb);
        }
        public void DialogueCreate(List<string> users) {
            var rb = new DialogueCreateBlock(users);
            bh.Send(rb);
        }

        private void RERAN() {
            client.Close();
            client = new Socket(IpAddress.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);
            bh = new BlockHelper(client);
            IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);
            client.Connect(remoteEP);
            recieveTask.Dispose();
            recieveTask = new Task(Recieve);
            recieveTask.Start();
        }


        public void RecieveOneBlock() {
            // var header = bh.RecieveHead();
            // switch (header.type) {
            //     case BlockTypeConstants.ErrorBlock:
            //         break;
            //     case BlockTypeConstants.ProfileBlock:
            //         RecieveProfile(header.size);
            //         break;
            //     case BlockTypeConstants.UserListBlock:
            //         RecieveUserList(header.size);
            //         break;
            //     case BlockTypeConstants.DialogueListBlock:
            //         RecievedialogueList(header.size);
            //         break;
            //     default:
            //         return;
            // }
        }

        public void Recieve() {
            try {
                var header = bh.RecieveHead();
                while ((header.type != 0 || header.size != 0) && header.type != BlockTypeConstants.ENDConnect) {
                    switch (header.type) {
                        case BlockTypeConstants.ErrorBlock:
                            break;
                        case BlockTypeConstants.ProfileBlock:
                            RecieveProfile(header.size);
                            break;
                        case BlockTypeConstants.UserListBlock:
                            RecieveUserList(header.size);
                            break;
                        case BlockTypeConstants.DialogueListBlock:
                            RecievedialogueList(header.size);
                            break;
                        case BlockTypeConstants.MsgListBlock:
                            RecievedMsgList(header.size);
                            break;
                        case BlockTypeConstants.SendMsgBlock:
                            RecievedMsg(header.size);
                            break;
                        default:
                            return;
                    }
                    while ((header = bh.RecieveHead()).size == -1) {
                        Thread.Sleep(1);
                    }
                }
            } catch (System.Net.Sockets.SocketException e) {
                RERAN();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public byte[] Recieve(int size) {
            var sb = new StringBuilder();
            var buffer = new byte[size];
            if (client.Receive(buffer, 0, size, 0) != size)
                return null;
            return buffer;
        }

        public void RecieveProfile(int size) {
            var tBlock = bh.RecieveBlock(size, new ProfileBlock());
            ProfileRecieved.Invoke(tBlock.Profile);
        }
        public void RecieveUserList(int size) {
            var tBlock = bh.RecieveBlock(size, new UserListBlock());
            UserListRecieved.Invoke(tBlock.Users);
        }
        public void RecievedialogueList(int size) {
            var tBlock = bh.RecieveBlock(size, new DialogueListBlock());
            DialogueListRecieved.Invoke(tBlock.Dialogues);
        }
        public void RecievedMsgList(int size) {
            var tBlock = bh.RecieveBlock(size, new MsgListBlock());
            MsgListRecieved.Invoke(tBlock.Messages);
        }
        public void RecievedMsg(int size) {
            var tBlock = bh.RecieveBlock(size, new MsgBlock());
            MsgRecieved.Invoke(tBlock.Message);
        }
    }

}
