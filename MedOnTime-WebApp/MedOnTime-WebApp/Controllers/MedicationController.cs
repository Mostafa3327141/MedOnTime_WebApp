using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MedOnTime_WebApp.Controllers
{
    public class MedicationController : Controller
    {

        private IMongoCollection<Medication> _medicationCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public MedicationController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _medicationCollection = database.GetCollection<Medication>("Medication");
        }

        public ActionResult MedicationList()
        {
            List<Medication> existingMeds = _medicationCollection.AsQueryable<Medication>().ToList();
            return View(existingMeds);
        }

        [HttpGet]
        public IActionResult MedicationForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MedicationForm(Medication formResponse)
        {
            if (ModelState.IsValid) 
            { 
                System.Diagnostics.Debug.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                try
                {
                    List<Medication> existingMeds = _medicationCollection.AsQueryable<Medication>().ToList();
                    if (existingMeds.Count == 0)
                        formResponse._id = 1;
                    else
                        formResponse._id = existingMeds[existingMeds.Count - 1]._id + 1;

                    _medicationCollection.InsertOne(formResponse);
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }
    }
}
