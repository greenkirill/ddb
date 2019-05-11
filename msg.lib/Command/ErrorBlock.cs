using System;

namespace msg.lib {
    public class ErrorBlock : MetaJson {
        public class ErrorBlockModel {
            public string Text;
        }
        
        public new const byte Type = BlockTypeConstants.ErrorBlock;

        
        public ErrorBlock(Exception e) :
            base(
                new ErrorBlockModel {
                    Text = e.ToString()
                }
                ) { }
        public ErrorBlock(string e) :
            base(
                new ErrorBlockModel {
                    Text = e
                }
                ) { }
        public ErrorBlock() : base() { }

        public string Text {
            get {
                return DJson.Text;
            }
        }
        
        public override byte[] GetBytes() {
            return GetBytes(ErrorBlock.Type);
        }
    }
}