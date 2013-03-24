using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationModifier
    {
        // HIGHDEA: Maybe instead of modifiers, it would be better to have locations that
        // are anchored to other locations.  Then, just store radians and distance from anchor
        // and bam!  The UI becomes a little less list/text-y that way too.

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LocationModifierId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Ordinal { get; set; }

        [Required]
        public int MedicationId { get; set; }
        public virtual Medication Medication { get; set; }
    }
}
