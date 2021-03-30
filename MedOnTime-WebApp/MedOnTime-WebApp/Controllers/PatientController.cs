using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/*
 For Adding Patients from Caretaker Account, 2 Steps:
    Allow caretaker to add patient once logged in and display patients if there's any
    Only allow med to be added in a patient profile
 */

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
        public IActionResult AddPatient(int PatientID, string FirstName, string LastName, string Email, int PhoneNum, int Age)
        {
            Console.WriteLine(FirstName);
            Patient formResponse = new Patient(PatientID, FirstName, LastName, Email, PhoneNum, Age);

            ViewBag.Message = "";
            if (formResponse != null)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + 
                                                    formResponse.Email + "," + formResponse.PhoneNum + "," + formResponse.Age);
                Console.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " +
                                                    formResponse.Email + "," + formResponse.PhoneNum + "," + formResponse.Age);
                try
                {
                    Console.WriteLine("See me starting try-catch?");
                    List<Patient> existingPatients = _patientCollection.AsQueryable<Patient>().ToList();

                    Console.WriteLine(existingPatients.Count);
                    foreach (var patient in existingPatients)
                    {
                        Console.WriteLine(patient.LastName);
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
                    _patientCollection.InsertOne(formResponse); // TODO: Verify that the patient is being inserted into the API
                }
                catch (MongoWriteConcernException) {
                    Console.WriteLine("Is there any errors?");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
