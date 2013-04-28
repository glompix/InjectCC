using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InjectCC.Web.ViewModels.Medication;
using InjectCC.Web.Controllers;

namespace InjectCC.Web.Test.Controllers
{
    [TestClass]
    public class MedicationControllerTest
    {
        private MedicationModel[] TestModels = new MedicationModel[]
        {
        };

        [TestMethod]
        public void Test_Medication_NewHasReferenceImages()
        {
            var controller = new MedicationController();
            controller.New();
        }

        [TestMethod]
        public void Test_Medication_CanCreate()
        {
            
        }
    }
}
