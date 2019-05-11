namespace msg.lib {
    public class ErrorBlock {
        public class ErrorBlockModel {
            public string Text;
        }
        
        public new const byte Type = BlockTypeConstants.ErrorBlock;

        
        public MsgBlock(Excception e) :
            base(
                new ErrorBlockModel {
                    Text = e.ToString()
                }
                ) { }
        public MsgBlock(string e) :
            base(
                new ErrorBlockModel {
                    Text = e
                }
                ) { }
        public MsgBlock() : base() { }
    }
}