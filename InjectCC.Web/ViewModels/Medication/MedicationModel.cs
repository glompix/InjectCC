using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;

namespace InjectCC.Web.ViewModels.Medication
{
    public abstract class MedicationModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public IList<Location> Locations { get; set; }

        public IList<string> ReferenceImages { get; set; }

        /// <param name="medication">The medication being created.</param>
        /// <param name="medications">All medications owned by the current user. (Inherited from ISettingsModel)</param>
        protected void LoadEntity(Model.Medication medication, IList<Location> locations)
        {
            Name = medication.Name;
            Description = medication.Description;
            Locations = locations;
        }
    }
}
