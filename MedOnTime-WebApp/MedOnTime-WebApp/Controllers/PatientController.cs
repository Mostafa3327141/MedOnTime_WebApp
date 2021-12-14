using MedOnTime_WebApp.Models;
using MedOnTime_WebApp.Views.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public PatientController()
        {
        }

        public async System.Threading.Tasks.Task<ActionResult> PatientLog(string Id, string patientFirstName)
        {
            // Fetch all the log of patient from API
            List<Log> existingLogs = new List<Log>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/LogAPI?patientID=" + Id + "&" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingLogs = JsonConvert.DeserializeObject<List<Log>>(apiRes);
                }
            }
            // passing the patient's firstname to view via view bag
            ViewBag.patientFirstName = patientFirstName;
            return View(existingLogs);
        }
        
        
        public async System.Threading.Tasks.Task<ActionResult> PatientList(string caretakerObjID)
        {
            Caretaker caretaker = await LoginStatus.LoadCaretaker(caretakerObjID);
            List<Patient> existingPatients = await LoginStatus.LoadPatients(caretaker.CaretakerID);
            return View(new PatientListViewModel { Patients = existingPatients, CaretakerID = caretaker.CaretakerID });
        }

        [HttpGet]
        public IActionResult AddPatient(int caretakerID, string caretakerObjID)
        {
            ViewBag.Message = "";
            return View(new AddPatientViewModel { CaretakerID = caretakerID, CaretakerObjID = caretakerObjID });
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> AddPatient(AddPatientViewModel formResponse)
        {
            // Hash the password
            using (SHA256 sha256hash = SHA256.Create())
                formResponse.Patient.Password = LoginStatus.GetHash(sha256hash, formResponse.TempPassword, formResponse.Patient.Email.ToLower());

            formResponse.Patient.IsPasswordTemporary = true;

            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.Patient.FirstName + ", " + formResponse.Patient.LastName + ", " +
                                                    formResponse.Patient.Email + ", " + formResponse.Patient.PhoneNum + ", " + formResponse.Patient.Age);
                try
                {
                    List<Patient> existingPatients = new List<Patient>();
                    // Get the existing Patients with GET method
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/PatientAPI?" + LoginStatus.ApiKey))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                            existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);
                        }
                    }

                    foreach (var patient in existingPatients)
                    {
                        Console.WriteLine(patient.LastName);
                        // if the response email is the same as an existing patient's email
                        if (formResponse.Patient.Email.Equals(patient.Email))
                        {
                            ViewBag.Message = formResponse.Patient.Email + " is associated to PID#" + patient.PatientID + ", " + patient.FirstName + ", " + patient.LastName + " already.";
                            return View(formResponse);
                        }
                    }

                    // if any existing patients, increment the new patient's patient ID
                    if (existingPatients.Count == 0)
                        formResponse.Patient.PatientID = 1;
                    else
                        formResponse.Patient.PatientID = existingPatients[existingPatients.Count - 1].PatientID + 1;
                    
                    formResponse.Patient.UnSelectedShapes = new List<Shape> {
                        new Shape { ShapeName = "circle", ShapeDisplay = "Circle" },
                        new Shape { ShapeName = "oval", ShapeDisplay = "Oval" },
                        new Shape { ShapeName = "triangle", ShapeDisplay = "Triangle" },
                        new Shape { ShapeName = "heart", ShapeDisplay = "Heart" },
                        new Shape { ShapeName = "pentagon", ShapeDisplay = "Pentagon" },
                        new Shape { ShapeName = "hexagon", ShapeDisplay = "Hexagon" },
                        new Shape { ShapeName = "octagon", ShapeDisplay = "Octagon" },
                        new Shape { ShapeName = "rightTri", ShapeDisplay = "Right Triangle" },
                        new Shape { ShapeName = "sTri", ShapeDisplay = "Scalene Triangle" },
                        new Shape { ShapeName = "square", ShapeDisplay = "Square" },
                        new Shape { ShapeName = "rectangle", ShapeDisplay = "Rectangle" },
                        new Shape { ShapeName = "parallelogram", ShapeDisplay = "Parallelogram" },
                        new Shape { ShapeName = "trapezuim", ShapeDisplay = "Trapezuim" },
                        new Shape { ShapeName = "rhombus", ShapeDisplay = "Rhombus" },
                        new Shape { ShapeName = "4star", ShapeDisplay = "4 Pointed Star" },
                        new Shape { ShapeName = "star", ShapeDisplay = "5 Pointed Star" },
                        new Shape { ShapeName = "6star", ShapeDisplay = "6 Pointed Star" }
                    };

                    // Add new Patient with POST method
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse.Patient), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync("https://medontime-api.herokuapp.com/api/PatientAPI?" + LoginStatus.ApiKey, content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }
                    return RedirectToAction("Index", "Home", new { caretakerObjID = formResponse.CaretakerObjID, caretakerID = formResponse.Patient.CaretakerID });
                }
                catch
                {
                    Console.WriteLine("Is there any errors?");
                }
            }
            return View(formResponse);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> PatientDetails(string Id)
        {
            // Request patient object with patient ID from API
            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id + "?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(patient);
        }

        // for going to EditPatient view to update info
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> EditPatient(string Id, int caretakerID)
        {
            // Request patient object with patient ID from API
            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id + "?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(new EditPatientViewModel { Patient = patient, CaretakerID = caretakerID});
        }

        // post update method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> EditPatient(EditPatientViewModel formResponse)
        {
            if (ModelState.IsValid)
            {
                // Update patient's info in the database via the API 
                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse.Patient), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", content))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("PatientList", new { caretakerID = formResponse.CaretakerID });
            }
            else
            {
                return View(formResponse);
            }
        } // end of EditPatient

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> DeletePatient(string Id)
        {
            // Request patient object with patient ID from API
            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id + "?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> DeletePatient(string Id, string caretakerObjID, Patient formResponse)
        {
            try
            {
                // Remove patient object from the database by patient ID via API
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id + "?" + LoginStatus.ApiKey))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                await LoginStatus.LoadPatients(formResponse.CaretakerID);
                return RedirectToAction("PatientList", new { caretakerObjID = caretakerObjID, caretakerID = formResponse.CaretakerID });
            }
            catch { return View(formResponse); }
        } // end of DeletePatient
    }
}
