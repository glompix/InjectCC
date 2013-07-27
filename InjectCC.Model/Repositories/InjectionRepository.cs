using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using InjectCC.Model.Domain;
using InjectCC.Model.Repositories;

namespace InjectCC.Model.EntityFramework
{
    public class InjectionRepository : BaseRepository
    {
        public InjectionRepository(Context context)
            : base(context)
        {
        }

        public Injection GetLatestFor(Medication medication)
        {
            var latestInjection = (from i in _context.Injections
                                   where i.MedicationId == medication.MedicationId
                                   orderby i.Date descending
                                   select i).FirstOrDefault();

            return latestInjection;
        }

        public Injection GetById(Guid id)
        {
            return _context.Injections.SingleOrDefault(i => i.InjectionId == id);
        }
    }
}
