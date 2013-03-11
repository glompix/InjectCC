using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationModifier
    {
        public int LocationModifierId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }

        public int LocationSetId { get; set; }
        public virtual LocationSet LocationSet { get; set; }
    }
}
