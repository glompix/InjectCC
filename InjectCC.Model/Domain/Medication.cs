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
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public Medication(Medication copyFrom)
        {
            if (copyFrom != null)
            {
                Name = copyFrom.Name;
                Description = copyFrom.Description;
                UserId = copyFrom.UserId;
                User = copyFrom.User;
            }
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

        public virtual IList<Location> Locations { get; set; }
        public virtual IList<LocationModifier> LocationModifiers { get; set; }
    }
}
