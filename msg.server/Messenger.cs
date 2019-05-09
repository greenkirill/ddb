using System;
using System.Collections.Generic;
using System.Net.Sockets;
using msg.lib;

namespace msg.server {
    public class Messenger {


        public List<MessengerSession> Sessions = new List<MessengerSession>();

        private object lockObj = new object();

        public void CreateNewSession(Socket client) {
            var session = new MessengerSession(client);
            session.MsgSended += SendMessage;
            session.SessionClosed += CloseSession;
            lock (lockObj) {
                Sessions.Add(session);
            }
            session.Start();
        }

        public void SendMessage(Guid SessionId, Message message) {
            RecheckSession();
            lock (lockObj) {
                foreach (var session in Sessions) {
                    try {
                        if (session.SessionId != SessionId) {
                            foreach (var memberId in message.Dialogue.Members) {
                                if (session.profile != null && memberId == session.profile.ID) {
                                    session.SendMsg(message);
                                }
                            }
                        }
                    } catch (Exception e) { }
                }
            }
        }
        public void CloseSession(Guid SessionId) {
            RecheckSession();
            lock (lockObj) {
                Sessions = Sessions.FindAll((x) => x.SessionId != SessionId);
            }
        }

        private void RecheckSession() {
            lock (lockObj) {
                Sessions = Sessions.FindAll((x) => x.client != null && BlockHelper.SocketConnected(x.client));
            }
        }
    }
}