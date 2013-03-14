using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;

namespace InjectCC.Web.ViewModels.Injection
{
    public class IndexModel
    {
        public Model.Injection NextInjection { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<LocationModifier> LocationModifiers { get; set; }

        public int Last90DaysRating { get; set; }
        public int Last30DaysRating { get; set; }
    }
}
