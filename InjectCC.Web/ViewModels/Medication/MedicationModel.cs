using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;

namespace InjectCC.Web.ViewModels.Medication
{
    public abstract class MedicationModel
    {
        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public List<Location> Locations { get; set; }

        public List<string> ReferenceImages { get; set; }

        /// <param name="medication">The medication being created.</param>
        protected void LoadEntity(Model.Medication medication, List<Location> locations)
        {
            MedicationId = medication.MedicationId;
            Name = medication.Name;
            Description = medication.Description;
            Locations = locations;
        }
    }
}
