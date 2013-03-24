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
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        public int MedicationId { get; set; }
        public virtual Medication Medication { get; set; }
        
        public Injection CalculateNext()
        {
            var next = new Injection();
            next.MedicationId = this.MedicationId;
            next.Date = this.Date + TimeSpan.FromMinutes(Location.MinutesUntilNextInjection);
            next.Location = this.Medication.Locations.FirstOrDefault(l => l.Ordinal > this.Location.Ordinal)
                ?? this.Medication.Locations.First();
            return next;
        }
    }
}