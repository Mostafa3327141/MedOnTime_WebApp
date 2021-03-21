using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

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
                        formResponse.MedID = 1;
                    else
                        formResponse.MedID = existingMeds[existingMeds.Count - 1].MedID + 1;

                    _medicationCollection.InsertOne(formResponse);
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }

        [HttpGet]
        public IActionResult MedicationEdit(int medID)
        {
            var medication = _medicationCollection.AsQueryable<Medication>().SingleOrDefault(x => x.MedID == medID);
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MedicationEdit(string objID, Medication formResponse)
        {
            try
            {
                var filter = Builders<Medication>.Filter.Eq("_id", ObjectId.Parse(objID));
                var update = Builders<Medication>.Update
                    .Set("MedicationName", formResponse.MedicationName)
                    .Set("MethodOfTaking", formResponse.MethodOfTaking)
                    .Set("Dosage", formResponse.Dosage)
                    .Set("MedicationType", formResponse.MedicationType)
                    .Set("Quantity", formResponse.Quantity);
                var result = _medicationCollection.UpdateOne(filter, update);
                return RedirectToAction("MedicationList");
            }
            catch { return View(formResponse); }
        }

        [HttpGet]
        public IActionResult MedicationDetails(int medID)
        {
            var medication = _medicationCollection.AsQueryable<Medication>().SingleOrDefault(x => x.MedID == medID);
            return View(medication);
        }

        [HttpGet]
        public IActionResult MedicationDelete(int medID)
        {
            var medication = _medicationCollection.AsQueryable<Medication>().SingleOrDefault(x => x.MedID == medID);
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MedicationDelete(string objID, Medication formResponse)
        {
            try
            {
                _medicationCollection.DeleteOne(Builders<Medication>.Filter.Eq("_id", ObjectId.Parse(objID)));
                return RedirectToAction("MedicationList");
            }
            catch { return View(formResponse); }
        }
    }
}
