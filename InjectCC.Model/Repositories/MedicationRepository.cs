using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.Domain;
using InjectCC.Model.Repositories;

namespace InjectCC.Model.EntityFramework
{
    // Going to be pretty basic crud
    public class MedicationRepository : Repository
    {
        public void Add(Medication medication)
        {
            Context.Medications.Add(medication);

            foreach (var location in medication.Locations ?? new List<Location>())
            {
                Context.Locations.Add(location);
            }
        }

        public Medication Find(int id)
        {
            return Context.Medications.SingleOrDefault(m => m.MedicationId == id);
        }

        public IList<Medication> ListAllForUser(int userId)
        {
            return Context.Medications.Where(m => m.UserId == userId).ToList();
        }

        public Medication GetFirstForUser(int userId)
        {
            return Context.Medications.FirstOrDefault(m => m.UserId == userId);
        }
    }
}
