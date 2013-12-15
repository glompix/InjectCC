using InjectCC.Model.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace InjectCC.Web.ViewModels.Medication
{
    public sealed class NewModel : SettingsBaseModel
    {
        private MedicationRepository _medications = new MedicationRepository();

        /// <summary>
        /// Represents a list of medications that the user may copy from.
        /// </summary>
        public IList<SelectListItem> CopyableMedications
        {
            get
            {
                // Provide a blank option at top of list to indicate "don't copy anything."
                var copyableOptions = _medications.ListCopyable().Select(m => new SelectListItem {
                    Text = m.Name,
                    Value = m.MedicationId.ToString()
                });
                copyableOptions = new SelectListItem[] { new SelectListItem() }.Union(copyableOptions);
                return new List<SelectListItem>(copyableOptions);
            }
        }

        /// <summary>
        /// The medication to copy.
        /// </summary>
        [Display(Name="Copy from")]
        public int? CopyFromMedicationId { get; private set; }

        /// <summary>
        /// The name of the medication. (e.g., Betaseron, Avonex...)
        /// </summary>
        [Display(Name="Medication Name"), Required(ErrorMessage="A name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// A short description of the medication. (e.g., "Twice weekly MS disease-modifying drug.")
        /// </summary>
        public string Description { get; set; }
    }
}
