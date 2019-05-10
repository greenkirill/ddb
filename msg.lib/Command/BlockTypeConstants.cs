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
        public const byte CSendMsgBlock = 44;
        public const byte ContactListBlock = 5;
        public const byte MsgListBlock = 6;
        public const byte TokenBlock = 7;
        public const byte ErrorBlock = 8;
        public const byte ProfileBlock = 9;
        public const byte ENDConnect = 255;

        public const byte DialogueCreateBlock = 10;
        public const byte DialogueListBlock = 11;
        public const byte RequestDialogueListBlock = 12;
        public const byte UserListBlock = 13;
        public const byte RequestUserListBlock = 14;
        public const byte RequestMsgList = 15;
        //public const byte FileIdPieceType = 31;
        //public const byte NamePieceType = 3;
        //public const byte PieceType = 5;
        //public const byte SizePieceType = 2;
        //public const byte TokenPieceType = 1;
        public const byte MetaBlock = 0;
    }
}
