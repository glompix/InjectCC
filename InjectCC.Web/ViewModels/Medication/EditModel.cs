using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using System.Collections.Generic;
using InjectCC.Model.Domain;
using AutoMapper;
using System.IO;

namespace InjectCC.Web.ViewModels.Medication
{
    public class EditModel : SettingsBaseModel
    {
        public int MedicationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public IList<Location> Locations { get; set; }

        public IList<string> ReferenceImages { get; set; }

        public EditModel(Model.Domain.Medication medication, IEnumerable<string> refImagePaths)
        {
            Mapper.Map(medication, this);
            ReferenceImages = new List<string>(refImagePaths);
        }
    }
}