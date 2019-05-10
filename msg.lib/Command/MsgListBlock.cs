using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace msg.lib {
    public class MsgListBlock : MetaJson {
        public class MsgListModel {
            public class Msg {
                public class Member {
                    public string ID;
                    public string Username;
                }

                public string ID;
                public string SentAt;
                public string SentBy;
                public string DialogId;
                public string text;
            }
            public List<Msg> msgs;
        }

        public new const byte Type = BlockTypeConstants.MsgListBlock;

        public MsgListBlock(List<Message> Messages) :
            base(
                new MsgListModel {
                    msgs = Messages.Select(Message => new MsgListModel.Msg {
                        ID = Message.ID.ToString(),
                        SentAt = Message.SentAt.ToString(),
                        SentBy = Message.SentBy.ToString(),
                        DialogId = Message.Dialogue.ID.ToString(),
                        text = Message.Text
                    }).ToList()

                }
                ) { }
        public MsgListBlock() : base() { }


        private Message DynamicToMessage(dynamic m) {
            return new Message {
                ID = Guid.Parse((string)m.ID),
                SentAt = DateTime.Parse((string)m.SentAt),
                SentBy = Guid.Parse((string)m.SentBy),
                Text = m.text,
                Dialogue = new Dialogue {
                    ID = Guid.Parse((string)m.DialogId)
                }
            };
        }

        public List<Message> Messages {
            get {
                var lm = new List<Message>();
                foreach (var item in DJson.msgs) {
                    lm.Add(DynamicToMessage(item));
                }
                return lm;
            }
        }


        public override byte[] GetBytes() {
            return GetBytes(MsgListBlock.Type);
        }


    }
}