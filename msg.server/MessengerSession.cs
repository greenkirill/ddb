using System;
using System.Threading;
using System.Net.Sockets;
using msg.lib;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace msg.server {
    public class MessengerSession : IMessengerSession {
        public Guid SessionId { get; } = Guid.NewGuid();

        public delegate void SendMsgEvent(Guid SessionId, Message message);
        public event SendMsgEvent MsgSended;
        public delegate void CloseSessionEvent(Guid SessionId);
        public event CloseSessionEvent SessionClosed;

        public Profile profile { get; private set; }
        public Socket client { get; }
        private BlockHelper bh;
        private MSGHelper mh;

        public MessengerSession(Socket client, MSGHelper mh) {
            this.client = client;
            bh = new BlockHelper(client);
            this.mh = mh;
        }

        public async Task StartAsync() {
            Start();
        }

        public void Start() {
            var header = bh.RecieveHead();
            // bool tokenRecieved = false;
            while (header.type != 0 && header.type != BlockTypeConstants.ENDConnect) {
                switch (header.type) {
                    case BlockTypeConstants.RegisterBlock:
                        RecieveRegister(header.size);
                        break;
                    case BlockTypeConstants.AuthBlock:
                        RecieveAuth(header.size);
                        break;
                    case BlockTypeConstants.RequestUserListBlock:
                        RecieveRequestUserList(header.size);
                        break;
                    case BlockTypeConstants.RequestDialogueListBlock:
                        RecieveRequestDialogueList(header.size);
                        break;
                    case BlockTypeConstants.DialogueCreateBlock:
                        RecieveDialogueCreate(header.size);
                        break;
                    case BlockTypeConstants.RequestMsgList:
                        RecieveRequestMsgList(header.size);
                        break;
                    case BlockTypeConstants.CSendMsgBlock:
                        RecieveMsg(header.size);
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
            try {
                profile = mh.CreateProfile(tBlock.Username, tBlock.Password);
                if (profile == null) {
                    client.Disconnect(false);
                    SessionClosed.Invoke(SessionId);
                } else {
                    SendProfile(profile);
                }

            } catch (Exception e) {
                SendError(e);
            }
        }

        private void RecieveAuth(int size) {
            var tBlock = bh.RecieveBlock(size, new AuthBlock());
            try {
                profile = mh.FindProfile(tBlock.Username, tBlock.Password);
                if (profile == null) {
                    client.Disconnect(false);
                    SessionClosed.Invoke(SessionId);
                } else {
                    SendProfile(profile);
                }
            } catch (Exception e) {
                SendError(e);
            }

        }
        private void RecieveRequestUserList(int size) {
            var tBlock = bh.RecieveBlock(size, new RequestUserListBlock());

            try {
                var users = mh.GetAllProfiles();
                SendUserList(users);
            } catch (Exception e) {
                SendError(e);
            }
        }
        private void RecieveRequestDialogueList(int size) {
            var tBlock = bh.RecieveBlock(size, new RequestDialogueList());

            try {
                var ds = mh.GetDialoguesByMemberId(profile.ID);
                SendDialogueList(ds);
            } catch (Exception e) {
                SendError(e);
            }
        }
        private void RecieveRequestMsgList(int size) {
            var tBlock = bh.RecieveBlock(size, new RequestMsgList());

            try {
                var ds = mh.GetMsgList(tBlock.DialogId, 0, 30);
                SendMsgList(ds);
            } catch (Exception e) {
                SendError(e);
            }
        }
        private void RecieveMsg(int size) {
            var tBlock = bh.RecieveBlock(size, new CMsgBlock());

            try {
                var m = mh.SendMessage(tBlock.DialogId, profile.ID, tBlock.text);
                MsgSended.Invoke(SessionId, m);
            } catch (Exception e) {
                SendError(e);
            }
        }
        private void RecieveDialogueCreate(int size) {
            var tBlock = bh.RecieveBlock(size, new DialogueCreateBlock());

            try {
                mh.CreateDialogue(tBlock.Users, profile.Username);
            } catch (Exception e) {
                SendError(e);
            }
        }

        public void SendProfile(Profile profile) {
            var rb = new ProfileBlock(profile);
            bh.Send(rb);
        }
        public void SendUserList(List<Profile> profiles) {
            var rb = new UserListBlock(profiles);
            bh.Send(rb);
        }
        public void SendMsgList(List<Message> msgs) {
            var rb = new MsgListBlock(msgs);
            bh.Send(rb);
        }
        public void SendDialogueList(List<Dialogue> dialogues) {
            var rb = new DialogueListBlock(dialogues);
            bh.Send(rb);
        }

        public void SendMsg(Message msg) {
            var rb = new MsgBlock(msg);
            bh.Send(rb);
        }

        private void SendError(Exception e) {
            var rb = new ErrorBlock("Something wrong with db. unlucky");
            bh.Send(rb);
        }


    }
}