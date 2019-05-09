using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib {
    public class MsgBlock : MetaJson {
        public class MsgModel {
            public Message Message;
        }

        public new const byte Type = BlockTypeConstants.SendMsgBlock;

        public MsgBlock(Message Message) :
            base(
                new MsgModel {
                    Message = Message
                }
                ) { }
        public MsgBlock(): base() {    }   
        
        public Message Message { get {
            return DJson.Message;
        }}
    }
}
