using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    /// <summary>
    /// Identifies a set of locations to be rotated through 
    /// </summary>
    public class LocationSet
    {
        public int LocationSetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Medication { get; set; }

        /// <summary>
        /// Optionally, the user that owns this LocationSet.  If not set, then
        /// this location set is public.
        /// </summary>
        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual List<Location> Locations { get; set; }

        public LocationSet CopyToUser(User user)
        {
            throw new NotImplementedException();
            return new LocationSet();
        }
    }
}
