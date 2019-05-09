using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib
{
    public class BlockTypeConstants
    {
        //public const byte EOFPieceType = 255;
        //public const byte HashPieceType = 4;
        public const byte MetaJson = 128;
        public const byte RegisterBlock = 1;
        public const byte AuthBlock = 2;
        public const byte RequestBlock = 3;
        public const byte SendMsgBlock = 4;
        public const byte ContactListBlock = 5;
        public const byte MsgListBlock = 6;
        public const byte TokenBlock = 7;
        public const byte ErrorBlock = 7;
        public const byte ENDConnect = 255;
        //public const byte FileIdPieceType = 31;
        //public const byte NamePieceType = 3;
        //public const byte PieceType = 5;
        //public const byte SizePieceType = 2;
        //public const byte TokenPieceType = 1;
        public const byte MetaBlock = 0;
    }
}
