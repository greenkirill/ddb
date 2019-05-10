namespace msg.lib {
    public class RequestDialogueList : MetaJson {
        public class RequestDialogueListModel {
        }

        public new const byte Type = BlockTypeConstants.RequestDialogueListBlock;

        public RequestDialogueList() :
            base(
                new RequestDialogueListModel {
                }
                ) { }

        public override byte[] GetBytes() {
            return GetBytes(RequestDialogueList.Type);
        }
    }
}