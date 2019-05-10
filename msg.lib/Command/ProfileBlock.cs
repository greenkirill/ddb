using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace msg.lib {
    public class ProfileBlock : MetaJson {
        public class ProfileModel {
            public Guid ID;
            public string Username;
            public string Password;
        }

        public new const byte Type = BlockTypeConstants.ProfileBlock;

        public ProfileBlock(Profile Profile) :
            base(
                new ProfileModel {
                    ID = Profile.ID,
                    Username = Profile.Username,
                    Password = Profile.Password
                }
                ) { }
        public ProfileBlock() : base() { }

        public Profile Profile {
            get {
                return new Profile {
                    ID = DJson.ID,
                    Username = DJson.Username,
                    Password = DJson.Password
                };
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(ProfileBlock.Type);
        }
    }
}
