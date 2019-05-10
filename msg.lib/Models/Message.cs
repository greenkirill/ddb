using System;
using System.ComponentModel.DataAnnotations;

namespace msg.lib {
    public class Message {

        [Key]
        public Guid ID { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        [Required]
        public Guid SentBy { get; set; }
        [Required]
        public Dialogue Dialogue { get; set; }
        [Required]
        public string Text { get; set; }
    }
}