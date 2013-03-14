using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model
{
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        public virtual List<LocationSet> LocationSets { get; set; }
    }
}
