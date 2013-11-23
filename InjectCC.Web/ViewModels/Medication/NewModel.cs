﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;
using InjectCC.Model.Domain;
using MedicationClass = InjectCC.Model.Domain.Medication;

namespace InjectCC.Web.ViewModels.Medication
{
    public class NewModel : ISettingsModel
    {
        /// <summary>
        /// Represents a list of medications that the user owns and may edit.
        /// </summary>
        public IList<MedicationClass> EditableMedications { get; set; }

        /// <summary>
        /// Represents a list of medications that the user may copy from.
        /// </summary>
        public IList<MedicationClass> CopyableMedications { get; set; }

        public NewModel(IEnumerable<MedicationClass> editableMedications, 
            IEnumerable<MedicationClass> copyableMedications)
        {
            this.EditableMedications = new List<MedicationClass>();
            this.CopyableMedications = new List<MedicationClass>();
        }
    }
}
