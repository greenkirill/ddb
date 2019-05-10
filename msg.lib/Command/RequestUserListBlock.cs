namespace msg.lib {
    public class RequestUserListBlock : MetaJson {
        public class RequestUserListBlockModel {
        }

        public new const byte Type = BlockTypeConstants.RequestUserListBlock;

        public RequestUserListBlock() :
            base(
                new RequestUserListBlockModel {
                }
                ) { }

        public override byte[] GetBytes() {
            return GetBytes(RequestUserListBlock.Type);
        }
    }
}