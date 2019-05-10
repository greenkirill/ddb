using System;
using System.Collections.Generic;
using System.Linq;
namespace msg.lib {
    public class DialogueListBlock : MetaJson {
        public class DialogueListBlockModel {
            public class Dialog {
                public class Member {
                    public class Profile {
                        public string ID;
                        public string Username;
                    }
                    public string ID;
                    public Profile profile;

                }
                public string ID;
                public List<Member> members;
            }
            public List<Dialog> Dialogues;
        }

        public new const byte Type = BlockTypeConstants.DialogueListBlock;

        public DialogueListBlock(List<Dialogue> Dialogues) :
            base(
                new DialogueListBlockModel {
                    Dialogues = Dialogues.Select(d =>
                        new DialogueListBlockModel.Dialog {
                            ID = d.ID.ToString(),
                            members = d.Members.Select(m =>

                                new DialogueListBlockModel.Dialog.Member {
                                    ID = m.MemberID.ToString(),
                                    profile = new DialogueListBlockModel.Dialog.Member.Profile {
                                        ID = m.profile.ID.ToString(),
                                        Username = m.profile.Username
                                    }
                                }).ToList()
                        }
                    ).ToList()
                }
                ) { }
        public DialogueListBlock() : base() { }

        public List<Dialogue> Dialogues {
            get {

                var ld = new List<DialogueListBlockModel.Dialog>();
                foreach (var item in DJson.Dialogues) {

                    var lm = new List<DialogueListBlockModel.Dialog.Member>();
                    foreach (var mm in item.members) {


                        lm.Add(new DialogueListBlockModel.Dialog.Member {
                            ID = mm.ID,
                            profile = new DialogueListBlockModel.Dialog.Member.Profile {
                                ID = mm.profile.ID,
                                Username = mm.profile.Username
                            }
                        });
                    }
                    ld.Add(new DialogueListBlockModel.Dialog {
                        ID = item.ID,
                        members = lm
                    });
                }
                return ld.Select(d => new Dialogue {
                    ID = Guid.Parse(d.ID),
                    Members = d.members.Select(m =>
                        new Member {
                            MemberID = Guid.Parse(m.ID),
                            profile = new Profile {
                                ID = Guid.Parse(m.profile.ID),
                                Username = m.profile.Username,
                                Password = ""
                            }

                        }).ToList()
                }).ToList();
            }
        }

        public override byte[] GetBytes() {
            return GetBytes(DialogueListBlock.Type);
        }
    }
}