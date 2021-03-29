using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

//TODO -> Test the AddPatient form with API connectivity and check update on MongoDB.

namespace MedOnTime_WebApp.Controllers
{
    public class PatientController : Controller
    {
        private IMongoCollection<Patient> _patientCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public PatientController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _patientCollection = database.GetCollection<Patient>("Patient");
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public IActionResult AddPatient(Patient formResponse)
        {
            Console.WriteLine(formResponse.FirstName);

            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + formResponse.Email + "," + formResponse.PhoneNum);
                try
                {
                    List<Patient> existingPatients = _patientCollection.AsQueryable<Patient>().ToList();
                    foreach (var patient in existingPatients)
                    {
                        // if the response email is the same as an existing patient's email
                        if (formResponse.Email.Equals(patient.Email))
                        {
                            ViewBag.Message = "Email " + formResponse.Email + " is not avaliable.";
                            return View(formResponse);
                        }
                    }

                    // if any existing patients, increment the new patient's patient ID
                    if (existingPatients.Count == 0)
                        formResponse.PatientID = 1;
                    else
                        formResponse.PatientID = existingPatients[existingPatients.Count - 1].PatientID + 1;
                    
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }
    }
}
