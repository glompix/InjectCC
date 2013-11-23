using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.EntityFramework;
using StructureMap;

namespace InjectCC.Model.Repositories
{
    public abstract class Repository
    {
        protected Context Context { get; private set; }

        protected Repository()
        {
            Context = ObjectFactory.GetInstance<Context>();
        }
    }
}
