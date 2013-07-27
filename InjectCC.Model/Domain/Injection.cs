using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model.Domain
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
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        public int MedicationId { get; set; }
        public virtual Medication Medication { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public Injection CalculateNext()
        {
            var next = new Injection();
            next.MedicationId = this.MedicationId;
            next.Date = this.Date + TimeSpan.FromMinutes(Location.MinutesUntilNextInjection);
            next.Location = this.Medication.Locations.OrderBy(l => l.Ordinal).FirstOrDefault(l => l.Ordinal > this.Location.Ordinal)
                ?? this.Medication.Locations.OrderBy(l => l.Ordinal).First();
            next.LocationId = next.Location.LocationId;
            return next;
        }
    }
}