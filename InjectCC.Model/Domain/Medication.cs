using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model.Domain
{
    /// <summary>
    /// Identifies a set of locations to be rotated through.
    /// </summary>
    public class Medication
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Medication()
        {
            Locations = new List<Location>();
            LocationModifiers = new List<LocationModifier>();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public Medication(Medication sourceMed)
            : this()
        {
            if (sourceMed != null)
            {
                Name = sourceMed.Name;
                Description = sourceMed.Description;
                UserId = sourceMed.UserId;
                User = sourceMed.User;
                foreach (var sourceLoc in sourceMed.Locations)
                {
                    var location = new Location(sourceLoc);
                    this.AddLocation(location);
                }
            }
        }

        private void AddLocation(Location location)
        {
            location.Medication = this;
            this.Locations.Add(location);
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// The user that owns this medication is public.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// The user that owns this medication is public.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        public virtual IList<Location> Locations { get; private set; }
        public virtual IList<LocationModifier> LocationModifiers { get; private set; }

        public Injection CalculateFirst()
        {
            var injection = new Injection();
            injection.MedicationId = this.MedicationId;
            injection.Date = DateTime.Now;
            injection.Location = this.Locations.OrderBy(l => l.Ordinal).First();
            injection.LocationId = injection.Location.LocationId;
            return injection;
        }

        public void AddLocation(Location location)
        {
            throw new NotImplementedException();
        }
    }
}
