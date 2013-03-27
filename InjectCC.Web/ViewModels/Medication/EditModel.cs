using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using System.Collections.Generic;

namespace InjectCC.Web.ViewModels.Medication
{
    public class EditModel : MedicationModel, ISettingsModel
    {
        /// <summary>
        /// From ISettingsModel.
        /// </summary>
        public IList<Model.Medication> Medications { get; set; }

        public static EditModel FromEntity(Model.Medication medication, IList<Location> locations, IList<Model.Medication> medications)
        {
            var model = new EditModel
            {
                Medications = medications
            };
            model.LoadEntity(medication, locations);
            return model;
        }
    }
}