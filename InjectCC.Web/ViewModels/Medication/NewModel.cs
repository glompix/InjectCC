﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using InjectCC.Model;

namespace InjectCC.Web.ViewModels.Medication
{
    public class NewModel : MedicationModel, ISettingsModel
    {
        /// <summary>
        /// From ISettingsModel.
        /// </summary>
        public IList<Model.Medication> Medications { get; set; }

        public static NewModel FromEntity(Model.Medication medication, IList<Location> locations, IList<Model.Medication> medications)
        {
            var model = new NewModel
            {
                Medications = medications
            };
            model.LoadEntity(medication, locations);
            return model;
        }
    }
}
