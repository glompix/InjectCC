using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The ordinal within the associated LocationSet. (for ordering)
        /// </summary>
        [Required]
        public int Ordinal { get; set; }

        /// <summary>
        /// The location set to which this Location belongs.
        /// </summary>
        [Required]
        public virtual LocationSet LocationSet { get; set; }
        public int LocationSetId { get; set; }
    }
}
