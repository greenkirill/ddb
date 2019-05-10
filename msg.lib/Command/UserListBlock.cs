using System;
using System.Collections.Generic;
using System.Linq;

namespace msg.lib {
    public class UserListBlock : MetaJson {
        public class UserListBlockModel {
            public class User {
                public string ID;
                public string Username;
            }
            public List<User> Users;
        }

        public new const byte Type = BlockTypeConstants.UserListBlock;

        public UserListBlock(List<Profile> Profiles) :
            base(
                new UserListBlockModel {
                    Users = Profiles
                    .Select(p => new UserListBlockModel.User { ID = p.ID.ToString(), Username = p.Username })
                    .ToList()
                }
                ) { }
        public UserListBlock() : base() { }

        public List<Profile> Users {
            get {
                var lusers = new List<UserListBlockModel.User>();
                foreach (var item in DJson.Users) {
                    lusers.Add(new UserListBlockModel.User {
                        ID = item.ID,
                        Username = item.Username
                    });
                }
                return lusers.Select(u => new Profile {
                    ID = Guid.Parse(u.ID),
                    Username = u.Username,
                    Password = ""
                }).ToList();
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(UserListBlock.Type);
        }
    }
}