using System;
using System.Collections.Generic;

namespace InjectCC.Web.ViewModels.Injection
{
    public class HistoryModel
    {
        public IEnumerable<Model.Domain.Injection> Injections { get; set; }
    }
}