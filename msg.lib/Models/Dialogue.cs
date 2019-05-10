using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace msg.lib {
    public class Dialogue {

        [Key]
        public Guid ID { get; set; }

        public List<Member> Members { get; set; }
    }
}