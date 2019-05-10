using System;

namespace msg.lib {
    public class RequestMsgList : MetaJson {
        public class RequestMsgListModel {
            public string DialogId;
        }

        public new const byte Type = BlockTypeConstants.RequestMsgList;

        public RequestMsgList(Guid DialogId) :
            base(
                new RequestMsgListModel {
                    DialogId = DialogId.ToString()
                }
                ) { }


        public Guid DialogId {
            get {
                return Guid.Parse((string)DJson.DialogId);
            }
        }
        public RequestMsgList() : base() { }
        public override byte[] GetBytes() {
            return GetBytes(RequestMsgList.Type);
        }
    }
}