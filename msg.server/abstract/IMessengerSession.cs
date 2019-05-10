using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using msg.lib;

namespace msg.server {
    public interface IMessengerSession {

        Profile profile { get; }
        Socket client { get; }
        Guid SessionId { get; }
        Task StartAsync();
        void Start();
        void SendMsg(Message msg);
    }
}