using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Web.ViewModels
{
    public interface ISettingsModel
    {
        IList<Model.Medication> Medications { get; set; }
    }
}
