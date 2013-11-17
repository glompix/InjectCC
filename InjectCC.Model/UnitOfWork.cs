using InjectCC.Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Model
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork()
        {
            Context = new Context();
        }

        public void Dispose()
        {
            Context.SaveChanges();
        }

        public Context Context { get; private set; }
    }
}
