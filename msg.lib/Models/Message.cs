using System;

namespace msg.lib {
    public class Message {
        public DateTime SentAt;
        public Guid SentBy;
        public Dialogue Dialogue;
        public string Text;
    }
}