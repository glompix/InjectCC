using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Web.ViewModels.Medication
{
    public class NewModel : ISettingsModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public IList<Model.Medication> Medications { get; set; }

        /// <param name="medication">The medication being created.</param>
        /// <param name="medications">All medications owned by the current user. (Inherited from ISettingsModel)</param>
        public static NewModel FromEntity(Model.Medication medication, IList<Model.Medication> medications)
        {
            return new NewModel
            {
                Name = medication.Name,
                Description = medication.Description,
                Medications = medications
            };
        }
    }
}
