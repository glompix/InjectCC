using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.EntityFramework;

namespace InjectCC.Model.Repositories
{
    public abstract class BaseRepository
    {
        protected Context Context { get { return _tx.Context; } }

        private UnitOfWork _tx;
        protected BaseRepository(UnitOfWork tx)
        {
            if (tx == null)
                throw new ArgumentNullException("context");

            _tx = tx;
        }
    }
}
