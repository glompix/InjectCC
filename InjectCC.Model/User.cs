using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual List<LocationSet> LocationSets { get; set; }
    }
}
