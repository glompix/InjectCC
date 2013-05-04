using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;

namespace InjectCC.Web.ViewModels.Medication
{
    public class NewModel : MedicationModel, ISettingsModel
    {
        /// <summary>
        /// Represents a list of medications that the user owns and may edit.
        /// </summary>
        public IList<Model.Medication> EditableMedications { get; set; }

        /// <summary>
        /// Represents a list of medications that the user may copy from.
        /// </summary>
        public IList<Model.Medication> CopyableMedications { get; set; }

        public static NewModel FromEntity(Model.Medication medication, 
            List<Location> locations, 
            List<Model.Medication> editableMedications, 
            List<Model.Medication> copyableMedications)
        {
            var model = new NewModel();
            model.EditableMedications = editableMedications;
            model.CopyableMedications = copyableMedications;
            model.LoadEntity(medication, locations);
            return model;
        }
    }
}
