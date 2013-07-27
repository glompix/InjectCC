using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model.Domain
{
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required, RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage="A valid email address is required.")]
        public string Email { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }
    }
}
