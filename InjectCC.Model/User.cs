using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InjectCC.Model
{
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        public virtual List<LocationSet> LocationSets { get; set; }
    }

    /*private User _userStub = new User
        {
            Email = "glompix@gmail.com",
            RegistrationDate = DateTime.Now,
            LocationSets = new List<LocationSet>
            {
                new LocationSet
                {
                    Name = "My Betaseron Injections",
                    Locations = new List<Location>
                    {
                        new Location { Name = "Right Abdomen", Ordinal = 1 },
                        new Location { Name = "Left Abdomen", Ordinal = 2 },
                        new Location { Name = "Right Thigh", Ordinal = 3 },
                        new Location { Name = "Left Thigh", Ordinal = 4 },
                        new Location { Name = "Right Buttock", Ordinal = 5 },
                        new Location { Name = "Left Buttock", Ordinal = 6 }
                    },
                    LocationModifiers = new List<LocationModifier>
                    {
                        new LocationModifier { Name = "High", Ordinal = 1 },
                        new LocationModifier { Name = "Middle", Ordinal = 2 },
                        new LocationModifier { Name = "Low", Ordinal = 3 }
                    }
                }
            }
        };*/
}
