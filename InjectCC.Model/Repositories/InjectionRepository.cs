using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using InjectCC.Model.Domain;
using InjectCC.Model.Repositories;

namespace InjectCC.Model.EntityFramework
{
    public class InjectionRepository : CrudRepository<Injection>
    {
        public InjectionRepository(UnitOfWork context)
            : base(context)
        {
        }

        public Injection GetLatestFor(Medication medication)
        {
            var latestInjection = (from i in Context.Injections
                                   where i.MedicationId == medication.MedicationId
                                   orderby i.Date descending
                                   select i).FirstOrDefault();

            return latestInjection;
        }

        public Injection GetById(Guid id)
        {
            return Context.Injections.SingleOrDefault(i => i.InjectionId == id);
        }

        public IList<Injection> GetAllForUser(int userId)
        {
            var injections = from i in Context.Injections
                             where i.UserId == userId
                             select i;
            return injections.ToList();
        }
    }
}
