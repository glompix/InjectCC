using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.EntityFramework;
using InjectCC.Model.Domain;

namespace InjectCC.Model.Repositories
{
    public class LocationRepository : Repository
    {
        public IList<Location> GetLocationsFor(Medication medication)
        {
            return Context.Locations.Where(l => l.Medication == medication).ToList();
        }
    }
}
