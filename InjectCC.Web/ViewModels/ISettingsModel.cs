using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InjectCC.Model.Domain;
using InjectCC.Model.EntityFramework;

namespace InjectCC.Web.ViewModels
{
    /// <summary>
    /// Base viewmodel class for settings area. (holds area tabs)
    /// </summary>
    public class SettingsBaseModel
    {
        private MedicationRepository _medications = new MedicationRepository();

        /// <summary>
        /// Represents a list of medications that the user owns and may edit.
        /// </summary>
        public IList<InjectCC.Model.Domain.Medication> EditableMedications
        {
            get
            {
                return _medications.ListAllForUser(0);
            }
        }
    }
}
