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
