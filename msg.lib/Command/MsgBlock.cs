using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msg.lib {
    public class MsgBlock : MetaJson {
        public class MsgModel {

            public class Member {
                public string ID;
                public string Username;
            }

            public string ID;
            public string SentAt;
            public string SentBy;
            public string DialogId;
            public List<Member> members;
            public string text;
        }

        public new const byte Type = BlockTypeConstants.SendMsgBlock;

        public MsgBlock(Message Message) :
            base(
                new MsgModel {
                    ID = Message.ID.ToString(),
                    SentAt = Message.SentAt.ToString(),
                    SentBy = Message.SentBy.ToString(),
                    DialogId = Message.Dialogue.ID.ToString(),
                    text = Message.Text,
                    members = Message.Dialogue.Members.Select(m => new MsgModel.Member {
                        ID = m.ID.ToString(),
                        Username = m.profile.Username
                    }).ToList()
                }
                ) { }
        public MsgBlock() : base() { }

        public Message Message {
            get {
                var mmbrs = DJson.members;
                var lm = new List<Member>();
                foreach (var item in mmbrs) {
                    lm.Add(new Member {
                        ID = Guid.Parse((string)item.ID),
                        profile = new Profile {
                            ID = Guid.Parse((string)item.ID),
                            Username = item.Username,
                            Password = ""
                        }
                    });
                }
                return new Message {
                    ID = Guid.Parse((string)DJson.ID),
                    SentAt = DateTime.Parse((string)DJson.SentAt),
                    SentBy = Guid.Parse((string)DJson.SentBy),
                    Text = DJson.text,
                    Dialogue = new Dialogue {
                        ID = Guid.Parse((string)DJson.DialogId),
                        Members = lm
                    }
                };
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(MsgBlock.Type);
        }
    }
}
