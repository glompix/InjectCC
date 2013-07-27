using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.EntityFramework;
using InjectCC.Model.Domain;

namespace InjectCC.Model.Repositories
{
    public class LocationRepository
    {
        private Context _context;
        public LocationRepository(Context context)
        {
            _context = context;
        }

        public IList<Location> GetLocationsFor(Medication medication)
        {
            return _context.Locations.Where(l => l.Medication == medication).ToList();
        }
    }
}
