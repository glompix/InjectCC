using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Model.Repositories
{
    public interface IMedicationRepository
    {
        void Create(Medication medication);
        Medication Find(int id);
        IList<Medication> ListAllForUser(int userId);
    }

    // Going to be pretty basic crud
    public class MedicationRepository : IMedicationRepository
    {
        protected Context _context;
        public MedicationRepository(Context context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Create(Medication medication)
        {
            _context.Medications.Add(medication);

            foreach (var location in medication.Locations)
            {
                _context.Locations.Add(location);
            }
        }

        public Medication Find(int id)
        {
            return _context.Medications.SingleOrDefault(m => m.MedicationId == id);
        }

        public IList<Medication> ListAllForUser(int userId)
        {
            return _context.Medications.Where(m => m.UserId == userId).ToList();
        }
    }
}
