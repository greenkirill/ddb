using System;
using System.Net.Sockets;
using msg.lib;

namespace msg.server {
    public interface IMessenger {
        void CreateNewSession(Socket client);
        void SendMessage(Guid SessionId, Message message);
        void CloseSession(Guid SessionId);
    }
}