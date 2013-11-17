using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using InjectCC.Model.Domain;

namespace InjectCC.Web.ViewModels.Medication
{
    public abstract class MedicationModel
    {
        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public IList<Location> Locations { get; set; }

        public IList<string> ReferenceImages { get; set; }

        /// <param name="medication">The medication being created.</param>
        protected void LoadEntity(Model.Domain.Medication medication, IList<Location> locations)
        {
            MedicationId = medication.MedicationId;
            Name = medication.Name;
            Description = medication.Description;
            Locations = locations;
        }
    }
}
