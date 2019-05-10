using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msg.lib {
    public class CMsgBlock : MetaJson {
        public class CMsgModel {
            public string DialogId;
            public string text;
        }

        public new const byte Type = BlockTypeConstants.CSendMsgBlock;

        public CMsgBlock(Guid did, string text) :
            base(
                new CMsgModel {
                    DialogId = did.ToString(),
                    text = text
                }
                ) { }
        public CMsgBlock() : base() { }

        public string text {
            get {
                return DJson.text;
            }
        }
        public Guid DialogId {
            get {
                return Guid.Parse((string)DJson.DialogId);
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(CMsgBlock.Type);
        }
    }
}
