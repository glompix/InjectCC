using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model
{
    public class Injection
    {
        public Injection()
        {
            InjectionId = Utilities.NewSequentialGUID();
        }

        [Key]
        public Guid InjectionId { get; private set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public Injection CalculateNext()
        {
            var next = new Injection();
            next.Date = this.Date + Location.TimeUntilNextInjection;
            next.User = this.User;
            next.Location = this.Location.Medication.Locations.FirstOrDefault(l => l.Ordinal > this.Location.Ordinal)
                ?? this.Location.Medication.Locations.First();
            return next;
        }
    }
}