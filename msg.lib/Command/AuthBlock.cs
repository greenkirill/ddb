using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib {
    public class AuthBlock : MetaJson {
        public class AuthModel {
            public string Username;
            public string Password;
        }

        public new const byte Type = BlockTypeConstants.AuthBlock;

        public AuthBlock(string username, string password) :
            base(
                new AuthModel {
                    Username = username,
                    Password = password
                }
                ) { }
        public AuthBlock(): base() {    }   
        
        public string Username { get {
            return DJson.Username;
        }}
        public string Password { get {
            return DJson.Password;
        }}
    }
}
