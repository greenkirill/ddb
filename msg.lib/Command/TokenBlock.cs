using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib {
    public class TokenBlock : MetaJson {
        public class TokenModel {
            public Token Token;
        }

        public new const byte Type = BlockTypeConstants.RegisterBlock;

        public TokenBlock(Token token) :
            base(
                new TokenModel {
                    Token = token
                }
                ) { }
        public TokenBlock(string token) :
            base(
                new TokenModel {
                    Token = new Token { TokenString = token, CreatedAt = DateTime.Now }
                }
                ) { }
        public TokenBlock() : base() { }


        public Token Token {
            get {
                return DJson.Token;
            }
        }
        public string TokenString {
            get {
                return DJson.Token.TokenString;
            }
        }
    }
}
