﻿using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using System.Collections.Generic;

namespace InjectCC.Web.ViewModels.Medication
{
    public class EditModel : MedicationModel, ISettingsModel
    {
        /// <summary>
        /// From ISettingsModel.
        /// </summary>
        public IList<Model.Medication> EditableMedications { get; set; }

        public static EditModel FromEntity(Model.Medication medication, List<Location> locations, List<Model.Medication> medications)
        {
            var model = new EditModel();
            model.EditableMedications = medications;
            model.LoadEntity(medication, locations);
            return model;
        }
    }
}