using System;
using System.Collections.Generic;

namespace InjectCC.Web.ViewModels.Injection
{
    public class HistoryModel
    {
        public IEnumerable<Model.Injection> Injections { get; set; }
    }
}