using System.Collections.Generic;
using System.Linq;

namespace msg.lib {
    public class DialogueCreateBlock : MetaJson {
        public class DialogueCreateBlockModel {
            public class User {
                public string Username;
            }
            public List<User> Users;
        }

        public new const byte Type = BlockTypeConstants.DialogueCreateBlock;

        public DialogueCreateBlock(List<string> users) :
            base(
                new DialogueCreateBlockModel {
                    Users = users
                    .Select(u => new DialogueCreateBlockModel.User { Username = u })
                    .ToList()
                }
                ) { }
        public DialogueCreateBlock() : base() { }

        public List<string> Users {
            get {
                var lusers = new List<DialogueCreateBlockModel.User>();
                foreach (var item in DJson.Users) {
                    lusers.Add(new DialogueCreateBlockModel.User {
                        Username = item.Username
                    });
                }

                return lusers.Select(u => u.Username).ToList();
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(DialogueCreateBlock.Type);
        }
    }
}