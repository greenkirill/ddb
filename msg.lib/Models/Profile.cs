using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace msg.lib
{
    public class Profile
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}