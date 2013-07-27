using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;
using InjectCC.Model.Domain;
using InjectionClass = InjectCC.Model.Domain.Injection;

namespace InjectCC.Web.ViewModels.Injection
{
    public class IndexModel
    {
        public InjectionClass Injection { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}
