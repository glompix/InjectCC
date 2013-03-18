using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model
{
    /// <summary>
    /// Identifies a set of locations to be rotated through 
    /// </summary>
    public class Medication
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Optionally, the user that owns this Medication.  If not set, then
        /// this location set is public.
        /// </summary>
        public virtual User User { get; set; }
        
        [Required]
        public int UserId { get; set; }

        public virtual List<Location> Locations { get; set; }
        public virtual List<LocationModifier> LocationModifiers { get; set; }
    }
}
