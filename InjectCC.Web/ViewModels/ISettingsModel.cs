using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InjectCC.Web.ViewModels
{
    /// <summary>
    /// Defines required members for viewmodels in the settings area.
    /// </summary>
    public interface ISettingsModel
    {
        IList<Model.Domain.Medication> EditableMedications { get; set; }
    }
}
