using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace msg.lib {
    public class ProfileBlock : MetaJson {
        public class ProfileModel {
            public Profile Profile;
        }

        public new const byte Type = BlockTypeConstants.ProfileBlock;

        public ProfileBlock(Profile Profile) :
            base(
                new ProfileModel {
                    Profile = Profile
                }
                ) { }
        public ProfileBlock(): base() {    }   
        
        public Profile Profile { get {
            return DJson.Profile;
        }}

        public override byte[] GetBytes() {
            return GetBytes(RegisterBlock.Type);
        }
    }
}
