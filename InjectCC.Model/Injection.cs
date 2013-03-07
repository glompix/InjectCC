using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    public class Injection
    {
        [Key]
        public Guid InjectionId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int LocationId { get; set; }
        // public virtual Location Location { get; set; }
    }
}