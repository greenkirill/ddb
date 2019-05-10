using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace msg.lib {
    public class RegisterBlock : MetaJson {
        public class RegisterModel {
            public string Username;
            public string Password;
        }

        public new const byte Type = BlockTypeConstants.RegisterBlock;

        public RegisterBlock(string username, string password) :
            base(
                new RegisterModel {
                    Username = username,
                    Password = password
                }
                ) { }
        public RegisterBlock(): base() {    }   
        
        public string Username { get {
            return DJson.Username;
        }}
        public string Password { get {
            return DJson.Password;
        }}

        public override byte[] GetBytes() {
            return GetBytes(RegisterBlock.Type);
        }
    }
}
