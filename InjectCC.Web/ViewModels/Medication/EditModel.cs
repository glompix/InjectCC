using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using System.Collections.Generic;
using InjectCC.Model.Domain;

namespace InjectCC.Web.ViewModels.Medication
{
    public class EditModel : SettingsBaseModel
    {
        /// <summary>
        /// From ISettingsModel.
        /// </summary>
        public IList<Model.Domain.Medication> EditableMedications { get; set; }

        public static EditModel FromEntity(Model.Domain.Medication medication, List<Model.Domain.Medication> medications)
        {
            var model = new EditModel();
            model.EditableMedications = medications;
            model.LoadEntity(medication);
            return model;
        }

        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public IList<Location> Locations { get; set; }

        public IList<string> ReferenceImages { get; set; }

        /// <param name="medication">The medication being created.</param>
        protected void LoadEntity(Model.Domain.Medication medication)
        {
            MedicationId = medication.MedicationId;
            Name = medication.Name;
            Description = medication.Description;
            Locations = medication.Locations;
        }
    }
}