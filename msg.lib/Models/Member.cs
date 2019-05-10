using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace msg.lib {
    public class Member {
        [Key]
        public Guid ID { get; set; }
        [NotMapped]
        public Profile profile { get; set; }
    }
}