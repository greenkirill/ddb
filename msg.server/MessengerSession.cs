using System;
using System.Threading;
using System.Net.Sockets;
using msg.lib;
using System.Threading.Tasks;

namespace msg.server {
    public class MessengerSession {
        public Guid SessionId { get; } = Guid.NewGuid();

        public delegate void SendMsgEvent(Guid SessionId, Message message);
        public event SendMsgEvent MsgSended;
        public delegate void CloseSessionEvent(Guid SessionId);
        public event CloseSessionEvent SessionClosed;

        public Profile profile { get ; private set; }
        public Socket client { get; }
        private BlockHelper bh;
        private MHelper mh;

        public MessengerSession(Socket client) {
            this.client = client;
            bh = new BlockHelper(client);
        }

        public async Task StartAsync() {
            Start();
        }

        public void Start() {
            var header = bh.RecieveHead();
            // bool tokenRecieved = false;
            while ((header.type != 0 || header.size != 0) && header.type != BlockTypeConstants.ENDConnect) {
                switch (header.type) {
                    case BlockTypeConstants.RegisterBlock:
                        RecieveRegister(header.size);
                        break;
                    case BlockTypeConstants.AuthBlock:
                        RecieveAuth(header.size);
                        break;
                    default:
                        return;
                }
                header = bh.RecieveHead();
            }
            SessionClosed.Invoke(SessionId);
        }


        private void RecieveRegister(int size) {
            var tBlock = bh.RecieveBlock(size, new RegisterBlock());
            profile = mh.RegisterProfile(tBlock.Username, tBlock.Password);
        }
        private void RecieveAuth(int size) {
            var tBlock = bh.RecieveBlock(size, new AuthBlock());
            profile = mh.AuthProfile(tBlock.Username, tBlock.Password);
        }

        public void SendMsg(Message msg) {
            var rb = new MsgBlock(msg);
            bh.Send(rb);
        }


    }
}