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
    public class LocationSet
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LocationSetId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public string MedicationName { get; set; }

        /// <summary>
        /// Optionally, the user that owns this LocationSet.  If not set, then
        /// this location set is public.
        /// </summary>
        public virtual User User { get; set; }
        [Required]
        public int UserId { get; set; }

        public virtual List<Location> Locations { get; set; }
        public virtual List<LocationModifier> LocationModifiers { get; set; }

        public LocationSet CopyToUser(User user)
        {
            throw new NotImplementedException();
            /*var set = new LocationSet(this);
            set.User = user;
            return set;*/
        }
    }
}
