using System;
using System.Collections.Generic;
using System.Linq;
using msg.lib;

namespace msg.server {
    public class MSGHelper {

        ShardDbContextFactory MSGContextFactory;
        Dictionary<Guid, Profile> CacheProfiles = new Dictionary<Guid, Profile>();

        public MSGHelper(List<string> MSGShardsConnectionStrings) {
            MSGContextFactory = new ShardDbContextFactory();
            this.MSGShardsConnectionStrings = MSGShardsConnectionStrings;
        }

        private List<string> MSGShardsConnectionStrings { get; }

        public Profile CreateProfile(string username, string password) {
            using (var context = new ProfileContext()) {
                var newProfile = new Profile {
                    Username = username,
                    Password = password,
                    ID = Guid.NewGuid()
                };
                context.Profiles.Add(newProfile);
                context.SaveChanges();
                return newProfile;
            }
        }
        public Profile FindProfile(string username, string password) {
            using (var context = new ProfileContext()) {
                var profile = from p in context.Profiles
                              where p.Username == username && p.Password == password
                              select p;
                if (profile.Count() == 1)
                    return profile.First();
                return null;
            }
        }

        public List<string> GetAllUsernames() {
            using (var context = new ProfileContext()) {
                return (from p in context.Profiles
                        select p.Username).ToList();
            }
        }

        public Dialogue CreateDialogue(List<string> usernames) {
            using (var context = new ProfileContext()) {
                var profiles = from p in context.Profiles
                               where usernames.Contains(p.Username) select p;
                if (profiles.Count() != usernames.Count)
                    throw new Exception("Incorrect username list");
                var profileIds = from p in profiles select p.ID;
                var newDialogue = new Dialogue {
                    ID = Guid.NewGuid(),
                    Members = profileIds.Select((x) => new Member { ID = x }).ToList()
                };
                using (var mContext = GetContextByGuid(newDialogue.ID)) {
                    mContext.Dialogues.Add(newDialogue);
                    mContext.SaveChanges();
                    return newDialogue;
                }
            }
        }

        public List<Dialogue> GetDialoguesByMemberId(Guid id) {
            var D = new List<Dialogue>();
            foreach (var s in MSGShardsConnectionStrings) {
                using (var context = MSGContextFactory.CreateDbContext(s)) {
                    D.Concat(
                        context.Dialogues.Where(d => d.Members.Any(m => m.ID == id))
                    );
                }
            }
            return D;
        }

        public List<Message> GetMsgList(Guid DId, int skip = 0, int take = 10) {
            using (var mContext = GetContextByGuid(DId)) {
                return mContext.Messages.Where(m => m.Dialogue.ID == DId).OrderByDescending(m => m.SentAt).Skip(skip).Take(take).ToList();
            }
        }

        public Message SendMessage(Guid DId, Guid MId, string text) {
            using (var context = GetContextByGuid(DId)) {
                var d = context.Dialogues.Where(D => D.ID == DId).First();
                var m = new Message {
                    ID = Guid.NewGuid(),
                    SentAt = DateTime.Now,
                    SentBy = MId,
                    Dialogue = d,
                    Text = text
                };
                context.Messages.Add(m);
                context.SaveChanges();
                return m;
            }
        }

        private Profile GetProfileById(Guid Id) {
            if (CacheProfiles.ContainsKey(Id))
                return CacheProfiles[Id];
            using (var context = new ProfileContext()) {
                var profile = from p in context.Profiles
                              where p.ID == Id
                              select p;
                if (profile.Count() == 1) {
                    CacheProfiles[Id] = profile.First();
                    return profile.First();
                }
                return null;
            }
        }

        private int GetContextIdByGuid(Guid id) {
            var bts = id.ToByteArray();
            var sum = 0;
            foreach (var b in bts) {
                sum += b;
            }
            return sum % MSGShardsConnectionStrings.Count;
        }

        private MSGContext GetContextByGuid(Guid id) {
            return MSGContextFactory.CreateDbContext(MSGShardsConnectionStrings[GetContextIdByGuid(id)]);
        }

    }
}