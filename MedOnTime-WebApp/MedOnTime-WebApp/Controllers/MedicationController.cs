using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

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

        public async System.Threading.Tasks.Task<ActionResult> MedicationList()
        {
            List<Medication> existingMeds = _medicationCollection.AsQueryable<Medication>().ToList();
            //List<Medication> existingMeds = new List<Medication>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/MedicationAPI"))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingMeds = JsonConvert.DeserializeObject<List<Medication>>(apiRes);
                }
            }
            return View(existingMeds);
        }

        [HttpGet]
        public IActionResult MedicationForm()
        {
            return View();
        }

        // Method for API Testing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationForm(Medication formResponse)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                Console.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync("https://localhost:44338/MedicationAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }

        // The commented out code is for testing the MongoDB Medication insert without the API
        /*[HttpPost]
        public IActionResult MedicationForm(Medication formResponse)
        {

            ViewBag.Message = "";
            if (formResponse != null)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                Console.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                try
                {
                    List<Medication> existingMedications = _medicationCollection.AsQueryable<Medication>().ToList();

                    *//*Console.WriteLine(existingMedications.Count);
                    foreach (var medication in existingMedications)
                    {
                        // if the response email is the same as an existing patient's email
                        if (formResponse.Email.Equals(patient.Email))
                        {
                            ViewBag.Message = "Email " + formResponse.Email + " is not avaliable.";
                            return View(formResponse);
                        }
                    }*//*

                    // if any existing patients, increment the new patient's patient ID
                    if (existingMedications.Count == 0)
                        formResponse.PatientID = 1;
                    else
                        formResponse.PatientID = existingMedications[existingMedications.Count - 1].PatientID + 1;
                    _medicationCollection.InsertOne(formResponse);
                }
                catch (MongoWriteConcernException)
                {
                    Console.WriteLine("Is there any errors?");
                }
            }
            return RedirectToAction("Index", "Home");
        }*/


        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(string Id)
        {

            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(string objID, Medication formResponse)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://localhost:44338/MedicationAPI", content))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("MedicationList");
            } 
            else
            {
                return View(formResponse);
            }
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDetails(string Id)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string Id)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string objID, Medication formResponse)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44338/MedicationAPI/" + objID))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("MedicationList");
            }
            catch { return View(formResponse); }
        }
    }
}
