using InjectCC.Model.EntityFramework;
using StructureMap;
using System;

namespace InjectCC.Web
{
    public class ComponentConfig
    {
        public static void Register()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<Context>().HybridHttpOrThreadLocalScoped().Use(() => new Context());
            });
        }
    }
}