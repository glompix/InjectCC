using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model
{
    /// <summary>
    /// Indicates a location on the body at which to inject medication.  Must
    /// be a part of a Medication.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public Location(Location loc, int? medicationId = null)
        {
            Name = loc.Name;
            Ordinal = loc.Ordinal;
            MinutesUntilNextInjection = loc.MinutesUntilNextInjection;
            ReferenceImageUrl = loc.ReferenceImageUrl;
            InjectionPointX = loc.InjectionPointX;
            InjectionPointY = loc.InjectionPointY;
            MedicationId = medicationId ?? loc.MedicationId;
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        /// <summary>
        /// The name of this injection site.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The ordinal within the associated Medication. (for ordering)
        /// Doesn't really matter if not in unbroken int sequence.
        /// </summary>
        [Required]
        public int Ordinal { get; set; }

        /// <summary>
        /// After injecting at this site, the time until the next injection is this value.
        /// </summary>
        [Required]
        public int MinutesUntilNextInjection { get; set; }

        /// <summary>
        /// The 
        /// </summary>
        [Required, MaxLength(250)]
        public string ReferenceImageUrl { get; set; }

        [Required, Range(0, 1000)]
        public int InjectionPointX { get; set; }

        [Required, Range(0, 1000)]
        public int InjectionPointY { get; set; }

        /// <summary>
        /// The location set to which this Location belongs.
        /// </summary>
        public virtual Medication Medication { get; set; }

        [Required]
        public int MedicationId { get; set; }
    }
}
