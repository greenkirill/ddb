using System;
using System.Collections.Generic;
using System.Net.Sockets;
using msg.lib;

namespace msg.server {
    public class Messenger : IMessenger {


        public List<IMessengerSession> Sessions = new List<IMessengerSession>();

        private object lockObj = new object();
        private MSGHelper mh;

        public Messenger(List<string> CStrings) {
            mh = new MSGHelper(CStrings);
        }

        public void CreateNewSession(Socket client) {
            var session = new MessengerSession(client, mh);
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
                            foreach (var member in message.Dialogue.Members) {
                                if (session.profile != null && member.ID == session.profile.ID) {
                                    session.SendMsg(message);
                                }
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        public void CloseSession(Guid SessionId) {
            RecheckSession();
            Console.WriteLine("CLOSE SESSION");
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