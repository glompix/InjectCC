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
            Context.SaveChanges();
        }

        public void Update(Medication medication)
        {
            Context.SaveChanges();
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

        public IList<Medication> ListCopyable()
        {
            return Context.Medications.ToList();
        }

        public Medication GetById(int id, int userId)
        {
            return Context.Medications.FirstOrDefault(m => m.MedicationId == id && m.UserId == userId);
        }
    }
}
