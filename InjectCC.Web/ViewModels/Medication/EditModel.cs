using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using System.Collections.Generic;
using InjectCC.Model.Domain;

namespace InjectCC.Web.ViewModels.Medication
{
    public class EditModel : MedicationModel, ISettingsModel
    {
        /// <summary>
        /// From ISettingsModel.
        /// </summary>
        public IList<Model.Domain.Medication> EditableMedications { get; set; }

        public static EditModel FromEntity(Model.Domain.Medication medication, List<Location> locations, List<Model.Domain.Medication> medications)
        {
            var model = new EditModel();
            model.EditableMedications = medications;
            model.LoadEntity(medication, locations);
            return model;
        }
    }
}